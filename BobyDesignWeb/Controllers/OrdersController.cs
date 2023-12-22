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
                FinishingDate = order.FinishingDate,
                JewelryShopId = order.Shop.Id,
                LaborPrice = order.LaborPrice,
                OrderCreatedOn = DateTime.UtcNow.ToBulgarianDateTime(),
                ShopUserId = userManager.GetUserId(User),
                OrderDescription = order.Description,
                Status = Data.Entities.OrderStatus.Opened,
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
                //string filePath = Path.Combine(webHostEnvironment.WebRootPath, imagesPathName, ordersPathName, fileName);
                //using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                //{
                //    await sketchBlob.CopyToAsync(fileStream);
                //}

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
                    FinishingDate = o.FinishingDate,
                    Status = (Models.OrderStatus)o.Status,
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
                    FinishingDate = o.FinishingDate,
                    Status = (Models.OrderStatus)o.Status,
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
