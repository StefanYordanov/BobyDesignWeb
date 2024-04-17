using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BobyDesignWeb.Controllers
{
    //[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
    public class SuppliersController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public SuppliersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<SupplierViewModel> GetAll()
        {
            return this._context.Suppliers.Select(js => new SupplierViewModel()
            {
                Id = js.SupplierId,
                Description = js.SupplierDescription,
                Name = js.SupplierName,
                PhoneNumbers = js.SupplierPhoneNumbers,
            });
        }

        [HttpGet]
        //[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public SupplierViewModel? GetDefaultActiveSupplier() 
        {
            return this._context.Suppliers
                .Select(s => new SupplierViewModel()
                {
                    Id = s.SupplierId,
                    Description = s.SupplierDescription,
                    Name = s.SupplierName,
                    PhoneNumbers = s.SupplierPhoneNumbers,
                }).FirstOrDefault();
        }

        //[HttpPost]
        ////[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        //public async Task<JewleryShopViewModel?> SetUserActiveShop(int? id)
        //{
        //    var currentUser = await userManager.GetUserAsync(User);
        //    currentUser.JewelryShopId = id;
        //    await userManager.UpdateAsync(currentUser);
        //    return this._context.Users.Where(u => u.Id == currentUser.Id)
        //        .Select(u => u.JewelryShop == null ? null : new JewleryShopViewModel()
        //        {
        //            Id = u.JewelryShop.JewelryShopId,
        //            Description = u.JewelryShop.JewelryShopDescription,
        //            Name = u.JewelryShop.JewelryShopName,
        //            PhoneNumbers = u.JewelryShop.JewelryShopPhoneNumbers,
        //        }).FirstOrDefault();
        //}
    }
}
