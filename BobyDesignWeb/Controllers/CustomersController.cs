using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BobyDesignWeb.Controllers
{
    [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public async Task<PageViewModel<CustomerViewModel>> Search(int page, string? searchPhrase)
        {
            page = page <= 0 ? 1 : page;
            int pageSize = 20;
            searchPhrase = searchPhrase != null ? searchPhrase.ToLower() : string.Empty;
            IQueryable<Customer> customersQuery = this._context.Customers
                .Where(x => x.CustomerName.ToLower().Contains(searchPhrase) ||
                x.CustomerEmail.ToLower().Contains(searchPhrase) ||
                x.CustomerPhone.ToLower().Contains(searchPhrase));

            int usersCount = customersQuery.Count();

            var users = await customersQuery.OrderBy(x => x.CustomerName).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new CustomerViewModel()
            {
                Id = x.CustomerId,
                Name = x.CustomerName,
                Email = x.CustomerEmail,
                PhoneNumber = x.CustomerPhone,
                TotalOrdersCost = x.Orders.Sum(x => x.TotalPrice),
                TotalPaidCost = x.Orders.Sum(x => x.Deposit),
            }).ToListAsync();

            return new PageViewModel<CustomerViewModel>()
            {
                Items = users,
                PagesCount = (int)Math.Ceiling(usersCount / Convert.ToDouble(pageSize)),
                CurrentPage = page,
            };
        }


        [HttpPost]
        [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public async Task<ActionResult> Create([FromBody] CustomerViewModel customer)
        {
            try
            {
                var customerEntity = new Customer()
                {
                    CustomerName = customer.Name,
                    CustomerEmail = customer.Email,
                    CustomerPhone = customer.PhoneNumber,
                };
                _context.Customers.Add(customerEntity);
                await _context.SaveChangesAsync();

                customer.Id = customerEntity.CustomerId;
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public async Task<ActionResult> Edit([FromBody] CustomerViewModel customer)
        {
            Customer? existingCustomer = await this._context.Customers
               .FirstOrDefaultAsync(x => x.CustomerId == customer.Id);

            if (existingCustomer == null)
            {
                return NotFound("Няма клиент със зададеното Id в базата данни");
            }

            existingCustomer.CustomerName = customer.Name;
            existingCustomer.CustomerEmail = customer.Email;
            existingCustomer.CustomerPhone = customer.PhoneNumber;
            await _context.SaveChangesAsync();

            return Ok(customer);

        }
    }
}
