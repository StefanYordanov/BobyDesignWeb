using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Data;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BobyDesignWeb.Services
{
    public class CustomersService
    {
        private readonly ApplicationDbContext _context;
        private readonly PaginationService _pagination;

        public CustomersService(ApplicationDbContext context, PaginationService pagination)
        {
            _context = context;
            this._pagination = pagination;
        }

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

            customersQuery = _pagination.GetPageItems(customersQuery, page);

            var customers = await customersQuery.Select(customer => new CustomerViewModel()
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
        public async Task<CustomerViewModel> Create(CustomerViewModel customer)
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
            return customer;
        }
        public async Task<CustomerReviewViewModel> CreateReview(CustomerReviewViewModel customerReview)
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
            return customerReview;
        }

        public async Task<CustomerViewModel> Edit(CustomerViewModel customer)
        {
            Customer? existingCustomer = await this._context.Customers
               .FirstOrDefaultAsync(x => x.CustomerId == customer.Id);

            if (existingCustomer == null)
            {
                throw new ArgumentException("Няма клиент със зададеното Id в базата данни");
            }

            existingCustomer.CustomerName = customer.Name;
            existingCustomer.CustomerEmail = customer.Email;
            existingCustomer.CustomerPhone = customer.PhoneNumber;
            await _context.SaveChangesAsync();
            return customer;

        }
    }
}
