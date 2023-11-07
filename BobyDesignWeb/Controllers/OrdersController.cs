using BobyDesignWeb.Data;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BobyDesignWeb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<OrderViewModel> GetOrders(DateTime? fromDate, DateTime? toDate, string? searchPhrase, int? customerId)
        {
            return OrdersQuery(fromDate, toDate, searchPhrase, customerId).ToList();
        }

        [HttpGet]
        public PageViewModel<OrderViewModel> GetOrdersPagination(int page, DateTime? fromDate, DateTime? toDate, string? searchPhrase, int? customerId)
        {
            page = page <= 0 ? 1 : page;
            int pageSize = 20;
            var ordersQuery = OrdersQuery(fromDate, toDate, searchPhrase, customerId);
            var ordersCount = ordersQuery.Count();
            var orders = ordersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PageViewModel<OrderViewModel>()
            {
                Items = orders,
                PagesCount = (int)Math.Ceiling(ordersCount / Convert.ToDouble(pageSize)),
                CurrentPage = page,
            };
        }

        private IQueryable<OrderViewModel> OrdersQuery(DateTime? fromDate, DateTime? toDate, string? searchPhrase, int? customerId)
        {
            fromDate ??= DateTime.MinValue;
            toDate ??= DateTime.MaxValue;
            searchPhrase = searchPhrase != null ? searchPhrase.ToLower() : string.Empty;

            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.ShopUser)
                .Where(o => o.FinishingDate.Date >= fromDate && o.FinishingDate.Date <= toDate && 
                    (customerId == null || customerId == o.CustomerId) && 
                    (
                        o.OrderDescription.ToLower().Contains(searchPhrase) ||
                        o.Customer.CustomerName.ToLower().Contains(searchPhrase) ||
                        o.Customer.CustomerEmail.ToLower().Contains(searchPhrase) ||
                        o.Customer.CustomerPhone.ToLower().Contains(searchPhrase)
                    )
                ).Select(o =>
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
                    Status = (OrderStatus)o.Status,
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
