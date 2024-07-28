using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using BobyDesignWeb.Services;
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
        private readonly CustomersService _customersService;

        public CustomersController(CustomersService customersService)
        {
            _customersService = customersService;
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public Task<CustomerViewModel?> GetCustomer(int id, bool getAllReviews = false)
        {
            return _customersService.GetCustomer(id, getAllReviews);
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public Task<PageViewModel<CustomerViewModel>> Search(int page, string? searchPhrase, bool getAllReviews = false)
        {
            return _customersService.Search(page, searchPhrase, getAllReviews);
        }


        [HttpPost]
        [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public async Task<ActionResult> Create([FromBody] CustomerViewModel customer)
        {
            try
            {
                var resultCustomer = await _customersService.Create(customer);
                return Ok(resultCustomer);
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
                var resultCustomerReview = await _customersService.CreateReview(customerReview);
                return Ok(resultCustomerReview);
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
            try
            {
                var resultCustomer = await _customersService.Edit(customer);
                return Ok(resultCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
