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
        public async Task<CustomerViewModel?> GetCustomer(int id, bool getAllReviews = false)
        {
            return await this._context.Customers.Select(customer => new CustomerViewModel()
            {
                Id = customer.CustomerId,
                Name = customer.CustomerName,
                Email = customer.CustomerEmail,
                PhoneNumber = customer.CustomerPhone,
                TotalOrdersCost = customer.Orders.Sum(x => x.TotalPrice),
                TotalPaidCost = customer.Orders.Sum(x => x.Deposit),
                Reviews = customer.CustomerReviews.Where(x => getAllReviews || x.CustomerReviewId == 
                    customer.CustomerReviews.OrderByDescending(cr=> cr.CreatedOn)
                    .Take(1)
                    .Select(cr => cr.CustomerReviewId)
                    .FirstOrDefault()
                
                ).OrderByDescending(cr => cr.CreatedOn).Select(x => new CustomerReviewViewModel()
                {
                    Id = x.CustomerReviewId,
                    CreatedOn = x.CreatedOn,
                    CustomerId = x.CustomerId,
                    Text = x.CustomerReviewText,
                    Type = x.CustomerReviewType
                }).ToList()
            }).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public async Task<PageViewModel<CustomerViewModel>> Search(int page, string? searchPhrase, bool getAllReviews = false)
        {
            page = page <= 0 ? 1 : page;
            int pageSize = 20;
            searchPhrase = searchPhrase != null ? searchPhrase.ToLower() : string.Empty;
            IQueryable<Customer> customersQuery = this._context.Customers
                .Where(x => x.CustomerName.ToLower().Contains(searchPhrase) ||
                x.CustomerEmail.ToLower().Contains(searchPhrase) ||
                x.CustomerPhone.ToLower().Contains(searchPhrase));

            int customersCount = customersQuery.Count();

            var customers = await customersQuery.OrderBy(x => x.CustomerName).Skip((page - 1) * pageSize).Take(pageSize).Select(customer => new CustomerViewModel()
            {
                Id = customer.CustomerId,
                Name = customer.CustomerName,
                Email = customer.CustomerEmail,
                PhoneNumber = customer.CustomerPhone,
                TotalOrdersCost = customer.Orders.Sum(x => x.TotalPrice),
                TotalPaidCost = customer.Orders.Sum(x => x.Deposit),
                Reviews = customer.CustomerReviews.Where(x => getAllReviews || x.CustomerReviewId ==
                    customer.CustomerReviews.OrderByDescending(cr => cr.CreatedOn)
                    .Take(1)
                    .Select(cr => cr.CustomerReviewId)
                    .FirstOrDefault()

                ).OrderByDescending(cr => cr.CreatedOn).Select(x => new CustomerReviewViewModel()
                {
                    Id = x.CustomerReviewId,
                    CreatedOn = x.CreatedOn,
                    CustomerId = x.CustomerId,
                    Text = x.CustomerReviewText,
                    Type = x.CustomerReviewType
                }).ToList()
            }).ToListAsync();

            return new PageViewModel<CustomerViewModel>()
            {
                Items = customers,
                PagesCount = (int)Math.Ceiling(customersCount / Convert.ToDouble(pageSize)),
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
        public async Task<ActionResult> CreateReview([FromBody] CustomerReviewViewModel customerReview)
        {
            try
            {
                var customerReviewEntity = new CustomerReview()
                {
                    CustomerId = customerReview.CustomerId,
                    CustomerReviewType = customerReview.Type,
                    CustomerReviewText = customerReview.Text,
                    CreatedOn = DateTime.Now,
                };
                _context.CustomerReviews.Add(customerReviewEntity);
                await _context.SaveChangesAsync();

                customerReview.Id = customerReviewEntity.CustomerReviewId;
                return Ok(customerReview);
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
