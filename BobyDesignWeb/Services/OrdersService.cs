﻿using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Data;
using BobyDesignWeb.Models;
using BobyDesignWeb.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BobyDesignWeb.Services
{
    public class OrdersService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public OrdersService(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<OrderViewModel> GetOrders(string? fromDate, string? toDate, string? searchPhrase, int? customerId, int? status, int? type)
        {

            return OrdersQuery(fromDate?.ToDateTime(), toDate?.ToDateTime(), searchPhrase, customerId, status, type).ToList();
        }

        public OrderViewModel GetOrder(int orderId)
        {
            return GetOrderDetails(orderId) ?? throw new Exception("Няма поръчка със зададеното id");
        }

        public PageViewModel<OrderViewModel> GetOrdersPagination(int page, string? fromDate, string? toDate, string? searchPhrase, int? customerId, int? status, int? type)
        {
            page = page <= 0 ? 1 : page;
            int pageSize = 20;
            var ordersQuery = OrdersQuery(fromDate?.ToDateTime(), toDate?.ToDateTime(), searchPhrase, customerId, status, type);
            var ordersCount = ordersQuery.Count();
            var orders = ordersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PageViewModel<OrderViewModel>()
            {
                Items = orders,
                PagesCount = (int)Math.Ceiling(ordersCount / Convert.ToDouble(pageSize)),
                CurrentPage = page,
            };
        }

        public int Pay(PayOrderQuery query)
        {
            var orderEntity = this._context.Orders.FirstOrDefault((o) => o.OrderId == query.OrderId) ?? throw new ArgumentException("Невалидно id на поръчка");
            if (orderEntity.Status == Data.Entities.OrderStatus.Closed)
            {
                throw new ArgumentException("Поръчката е вече затворена");
            }

            if (orderEntity.TotalPrice - orderEntity.Deposit < query.Payment)
            {
                throw new ArgumentException("Плащате повече от необходимото");
            }

            orderEntity.Deposit += query.Payment;
            if (orderEntity.Deposit == orderEntity.TotalPrice)
            {
                orderEntity.Status = Data.Entities.OrderStatus.Closed;
            }
            this._context.SaveChanges();
            return orderEntity.OrderId;
        }

        [HttpPost]
        public async Task<OrderViewModel?> UpdateOrder(SubmitEditOrderModel order, IFormFile? sketchBlob)
        {
            var orderEntity = this._context.Orders.Include(o => o.OrderCraftingComponents).FirstOrDefault(o => o.OrderId == order.Model.Id);
            if (orderEntity == null)
            {
                throw new ArgumentException(nameof(order), "Невалидно Id");
            }

            orderEntity.CustomerId = order.Model.Customer.Id;
            orderEntity.Deposit = order.Model.Deposit;
            orderEntity.FinishingDate = order.Model.FinishingDate.ToDateTime();
            orderEntity.JewelryShopId = order.Model.Shop.Id;
            orderEntity.LaborPrice = order.Model.LaborPrice;
            orderEntity.OrderDescription = order.Model.Description;
            orderEntity.Status = (Data.Entities.OrderStatus)order.Model.Status;
            orderEntity.PaymentMethod = order.Model.PaymentMethod;
            orderEntity.TotalPrice = order.Model.TotalPrice;

            var craftingComponentsToBeDeleted = orderEntity.OrderCraftingComponents.Where(cc => order.DeletedCraftingComponentIds.Contains(cc.OrderCraftingComponentId));
            this._context.OrderCraftingComponents.RemoveRange(craftingComponentsToBeDeleted);
            foreach (var ccModel in order.Model.CraftingComponents)
            {
                if (ccModel.Id > 0)
                {
                    var existingCraftingComponentEntity = orderEntity.OrderCraftingComponents
                        .FirstOrDefault(ccEntity => ccEntity.OrderCraftingComponentId == ccModel.Id);
                    if (existingCraftingComponentEntity == null)
                    {
                        continue;
                    }

                    existingCraftingComponentEntity.WorkMaterialPrice = ccModel.WorkMaterialPrice;
                    existingCraftingComponentEntity.WorkMaterialId = ccModel.WorkMaterial.Id;
                    existingCraftingComponentEntity.WorkMaterialQuantity = ccModel.Quantity;
                    existingCraftingComponentEntity.TotalComponentPrice = ccModel.TotalComponentPrice;
                    existingCraftingComponentEntity.IsDeposit = ccModel.IsDeposit;
                }
                else
                {
                    OrderCraftingComponent newCraftingComponentEntity = new()
                    {
                        WorkMaterialPrice = ccModel.WorkMaterialPrice,
                        WorkMaterialId = ccModel.WorkMaterial.Id,
                        WorkMaterialQuantity = ccModel.Quantity,
                        TotalComponentPrice = ccModel.TotalComponentPrice,
                        IsDeposit = ccModel.IsDeposit
                    };

                    orderEntity.OrderCraftingComponents.Add(newCraftingComponentEntity);
                }
            }

            _context.SaveChanges();

            if (sketchBlob != null && sketchBlob.ContentType == "image/png" && sketchBlob.Length < 2000000)
            {
                var fileName = Guid.NewGuid().ToString() + ".png";
                var storedFileEntity = sketchBlob.ToEntity(fileName);
                var existingStoredFileId = orderEntity.StoredFileId;
                orderEntity.StoredFile = storedFileEntity;

                orderEntity.ImageFileName = fileName;
                _context.SaveChanges();
                if (existingStoredFileId.HasValue)
                {
                    var oldStoredFile = new StoredFile() { Id = existingStoredFileId.Value };
                    this._context.StoredFiles.Attach(oldStoredFile);
                    this._context.StoredFiles.Remove(oldStoredFile);
                    this._context.SaveChanges();
                }

                return GetOrderDetails(orderEntity.OrderId);
            }
            return GetOrderDetails(orderEntity.OrderId);
        }

        public async Task<OrderViewModel?> InsertOrder(OrderViewModel order, IFormFile? sketchBlob, ClaimsPrincipal user)
        {
            var orderEntity = new Order()
            {
                CustomerId = order.Customer.Id,
                Deposit = order.Deposit,
                FinishingDate = order.FinishingDate.ToDateTime(),
                JewelryShopId = order.Shop.Id,
                SupplierId = order.Supplier.Id,
                LaborPrice = order.LaborPrice,
                OrderCreatedOn = DateTime.UtcNow.ToBulgarianDateTime(),
                ShopUserId = userManager.GetUserId(user),
                OrderDescription = order.Description,
                Status = Data.Entities.OrderStatus.Opened,
                PaymentMethod = order.PaymentMethod,
                NotifyCustomer = false,
                TotalPrice = order.TotalPrice,
                OrderType = order.Type,
                LinkedOrderId = order.LinkedOrderId,
                OrderCraftingComponents = order.CraftingComponents
                .Select(x => new OrderCraftingComponent()
                {
                    TotalComponentPrice = x.TotalComponentPrice,
                    IsDeposit = x.IsDeposit,
                    WorkMaterialId = x.WorkMaterial.Id,
                    WorkMaterialPrice = x.WorkMaterialPrice,
                    WorkMaterialQuantity = x.Quantity
                }).ToList(),
                ImageFileName = "",

            };

            _context.Add(orderEntity); ;
            _context.SaveChanges();

            if (sketchBlob != null && sketchBlob.ContentType == "image/png" && sketchBlob.Length < 2000000)
            {
                var fileName = Guid.NewGuid().ToString() + ".png";
                var storedFileEntity = sketchBlob.ToEntity(fileName);

                orderEntity.StoredFile = storedFileEntity;

                orderEntity.ImageFileName = fileName;
                _context.SaveChanges();

                return GetOrderDetails(orderEntity.OrderId);
            }
            return GetOrderDetails(orderEntity.OrderId);
        }

        public OrderViewModel? GetOrderDetails(int orderId)
        {
            return this._context.Orders.Where(o => o.OrderId == orderId)
                .Select(o => new OrderViewModel()
                {
                    Id = o.OrderId,
                    Deposit = o.Deposit,
                    CreatedOn = o.OrderCreatedOn,
                    Customer = new CustomerViewModel()
                    {
                        Email = o.Customer.CustomerEmail,
                        Id = o.Customer.CustomerId,
                        Name = o.Customer.CustomerName,
                        PhoneNumber = o.Customer.CustomerPhone,
                        Reviews = o.Customer.CustomerReviews
                            .OrderByDescending(cr => cr.CreatedOn).Take(1)
                            .Select(x => new CustomerReviewViewModel()
                            {
                                Id = x.CustomerReviewId,
                                CreatedOn = x.CreatedOn,
                                CustomerId = x.CustomerId,
                                Text = x.CustomerReviewText,
                                Type = x.CustomerReviewType
                            }).ToList()

                    },
                    Description = o.OrderDescription,
                    FinishingDate = o.FinishingDate.ToDateOnlyModel(),
                    Status = (Models.OrderStatus)o.Status,
                    PaymentMethod = o.PaymentMethod,
                    TotalPrice = o.TotalPrice,
                    LaborPrice = o.LaborPrice,
                    ShopUser = new UserViewModel()
                    {
                        Id = o.ShopUserId,
                        Email = o.ShopUser.Email,
                        FirstName = o.ShopUser.FirstName,
                        LastName = o.ShopUser.LastName,
                        PhoneNumber = o.ShopUser.PhoneNumber,
                        UserName = o.ShopUser.UserName,
                    },
                    Shop = new JewleryShopViewModel()
                    {
                        Id = o.JewelryShopId,
                        Description = o.JewelryShop.JewelryShopDescription,
                        Name = o.JewelryShop.JewelryShopName,
                        PhoneNumbers = o.JewelryShop.JewelryShopPhoneNumbers
                    },
                    Supplier = new SupplierViewModel()
                    {
                        Id = o.SupplierId,
                        Description = o.Supplier.SupplierDescription,
                        Name = o.Supplier.SupplierName,
                        PhoneNumbers = o.Supplier.SupplierPhoneNumbers
                    },
                    ImageFileName = o.StoredFile == null ? null : "/storedFiles/get?fileName=" + o.StoredFile.FileName,
                    LinkedOrderId = o.LinkedOrderId,
                    Type = o.OrderType,
                    CraftingComponents = o.OrderCraftingComponents.Select(occ => new OrderCraftingComponentViewModel()
                    {
                        Id = occ.OrderCraftingComponentId,
                        IsDeposit = occ.IsDeposit,
                        Quantity = occ.WorkMaterialQuantity,
                        TotalComponentPrice = occ.TotalComponentPrice,
                        WorkMaterialPrice = occ.WorkMaterialPrice,
                        WorkMaterial = new WorkMaterialModel()
                        {
                            Id = occ.WorkMaterial.WorkMaterialId,
                            Name = occ.WorkMaterial.WorkMaterialName,
                            MeasuringUnit = occ.WorkMaterial.WorkMaterialMeasuringUnit,
                            PricingType = occ.WorkMaterial.WorkMaterialPricingType,
                            Quantity = occ.WorkMaterial.Quantity,
                            ReservedQuantity = occ.WorkMaterial.ReservedQuantity,
                            RelevantPrice = occ.WorkMaterial.WorkMaterialPriceForDates.OrderByDescending(x => x.Date)
                            .Where(x => x.Date < o.OrderCreatedOn)
                            .Select(x => new LatestWorkMaterialRelevantPriceModel()
                            {
                                LastUpdatedOn = x.Date,
                                SellingPrice = x.SellingPrice,
                                PurchasingPrice = x.PurchasingPrice,
                                WorkMaterialId = x.WorkMaterialId,
                                Id = x.WorkMaterialPriceForDateId,
                            }).FirstOrDefault()
                        }
                    }).ToList()
                })
                .FirstOrDefault();
        }

        public IQueryable<OrderViewModel> OrdersQuery(DateTime? fromDate, DateTime? toDate, string? searchPhrase, int? customerId, int? status, int? type)
        {
            fromDate ??= DateTime.MinValue;
            toDate ??= DateTime.MaxValue;
            searchPhrase = searchPhrase != null ? searchPhrase.ToLower() : string.Empty;

            return _context.Orders
                .Where(o => o.FinishingDate.Date >= fromDate && o.FinishingDate.Date <= toDate &&
                (status == null || o.Status == (Data.Entities.OrderStatus)status) &&
                    (type == null || o.OrderType == (Data.Entities.OrderType)type) &&
                    (customerId == null || customerId == o.CustomerId) &&
                    (
                        o.OrderDescription.ToLower().Contains(searchPhrase) ||
                        o.Customer.CustomerName.ToLower().Contains(searchPhrase) ||
                        o.Customer.CustomerEmail.ToLower().Contains(searchPhrase) ||
                        o.Customer.CustomerPhone.ToLower().Contains(searchPhrase)
                    )
                ).OrderByDescending(o => o.OrderCreatedOn).Select(o =>
                new OrderViewModel()
                {
                    Id = o.OrderId,
                    Deposit = o.Deposit,
                    CreatedOn = o.OrderCreatedOn,
                    Customer = new CustomerViewModel()
                    {
                        Email = o.Customer.CustomerEmail,
                        Id = o.Customer.CustomerId,
                        Name = o.Customer.CustomerName,
                        PhoneNumber = o.Customer.CustomerPhone,
                        Reviews = o.Customer.CustomerReviews
                            .OrderByDescending(cr => cr.CreatedOn).Take(1)
                            .Select(x => new CustomerReviewViewModel()
                            {
                                Id = x.CustomerReviewId,
                                CreatedOn = x.CreatedOn,
                                CustomerId = x.CustomerId,
                                Text = x.CustomerReviewText,
                                Type = x.CustomerReviewType
                            }).ToList()
                    },
                    Description = o.OrderDescription,
                    FinishingDate = o.FinishingDate.ToDateOnlyModel(),
                    Status = (Models.OrderStatus)o.Status,
                    PaymentMethod = o.PaymentMethod,
                    TotalPrice = o.TotalPrice,
                    LaborPrice = o.LaborPrice,
                    ShopUser = new UserViewModel()
                    {
                        Id = o.ShopUserId,
                        Email = o.ShopUser.Email,
                        FirstName = o.ShopUser.FirstName,
                        LastName = o.ShopUser.LastName,
                        PhoneNumber = o.ShopUser.PhoneNumber,
                        UserName = o.ShopUser.UserName,
                    }
                });
        }
    }
}
