using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using BobyDesignWeb.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BobyDesignWeb.Controllers
{
    [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public OrdersController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IEnumerable<OrderViewModel> GetOrders(DateTime? fromDate, DateTime? toDate, string? searchPhrase, int? customerId, int? status)
        {
            return OrdersQuery(fromDate, toDate, searchPhrase, customerId, status).ToList();
        }

        [HttpGet]
        public OrderViewModel GetOrder(int orderId)
        {
            return GetOrderDetails(orderId) ?? throw new Exception("Няма поръчка със зададеното id");
        }

        [HttpGet]
        public PageViewModel<OrderViewModel> GetOrdersPagination(int page, DateTime? fromDate, DateTime? toDate, string? searchPhrase, int? customerId, int? status)
        {
            page = page <= 0 ? 1 : page;
            int pageSize = 20;
            var ordersQuery = OrdersQuery(fromDate, toDate, searchPhrase, customerId, status);
            var ordersCount = ordersQuery.Count();
            var orders = ordersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PageViewModel<OrderViewModel>()
            {
                Items = orders,
                PagesCount = (int)Math.Ceiling(ordersCount / Convert.ToDouble(pageSize)),
                CurrentPage = page,
            };
        }

        [HttpPost]
        public ActionResult Pay([FromBody] PayOrderQuery query)
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
            return Ok(GetOrderDetails(orderEntity.OrderId));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromForm] string model, IFormFile? sketchBlob)
        {
            var order = JsonConvert.DeserializeObject<SubmitEditOrderModel>(model);

            if (order == null || order.Model == null)
            {
                throw new ArgumentNullException(nameof(model), "Невалиден модел");
            }

            var orderEntity = this._context.Orders.Include(o => o.OrderCraftingComponents).FirstOrDefault(o => o.OrderId == order.Model.Id);
            if (orderEntity == null)
            {
                throw new ArgumentException(nameof(model), "Невалидно Id");
            }

            orderEntity.CustomerId = order.Model.Customer.Id;
            orderEntity.Deposit = order.Model.Deposit;
            orderEntity.FinishingDate = order.Model.FinishingDate.ToBulgarianDateTime();
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

                return Ok(GetOrderDetails(orderEntity.OrderId));
            }
            return Ok(GetOrderDetails(orderEntity.OrderId));
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrder([FromForm] string model, IFormFile? sketchBlob)
        {
            var order = JsonConvert.DeserializeObject<OrderViewModel>(model);

            if (order == null)
            {
                throw new ArgumentNullException(nameof(model), "Невалиден модел");
            }

            var orderEntity = new Order()
            {
                CustomerId = order.Customer.Id,
                Deposit = order.Deposit,
                FinishingDate = order.FinishingDate.ToBulgarianDateTime(),
                JewelryShopId = order.Shop.Id,
                LaborPrice = order.LaborPrice,
                OrderCreatedOn = DateTime.UtcNow.ToBulgarianDateTime(),
                ShopUserId = userManager.GetUserId(User),
                OrderDescription = order.Description,
                Status = Data.Entities.OrderStatus.Opened,
                PaymentMethod = order.PaymentMethod,
                NotifyCustomer = false,
                TotalPrice = order.TotalPrice,
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
                
                return Ok(GetOrderDetails(orderEntity.OrderId));
            }
            return Ok(GetOrderDetails(orderEntity.OrderId));
        }

        private OrderViewModel? GetOrderDetails(int orderId)
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
                    },
                    Description = o.OrderDescription,
                    FinishingDate = o.FinishingDate.ToBulgarianDateTime(),
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
                    Shop= new JewleryShopViewModel()
                    {
                        Id = o.JewelryShopId,
                        Description = o.JewelryShop.JewelryShopDescription,
                        Name = o.JewelryShop.JewelryShopName,
                        PhoneNumbers = o.JewelryShop.JewelryShopPhoneNumbers
                    },
                    ImageFileName = o.StoredFile == null ? null : "/storedFiles/get?fileName=" + o.StoredFile.FileName,
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

        private void SaveFileStream(String path, Stream stream)
        {
            var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            fileStream.Dispose();
        }

        private IQueryable<OrderViewModel> OrdersQuery(DateTime? fromDate, DateTime? toDate, string? searchPhrase, int? customerId, int? status)
        {
            fromDate ??= DateTime.MinValue;
            toDate ??= DateTime.MaxValue;
            searchPhrase = searchPhrase != null ? searchPhrase.ToLower() : string.Empty;

            return _context.Orders
                .Where(o => o.FinishingDate.Date >= fromDate && o.FinishingDate.Date <= toDate && 
                (status == null || o.Status == (Data.Entities.OrderStatus)status) &&
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
                    },
                    Description = o.OrderDescription,
                    FinishingDate = o.FinishingDate.ToBulgarianDateTime(),
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
