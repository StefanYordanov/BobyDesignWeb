using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BobyDesignWeb.Controllers
{
    //[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
    public class JewelryShopsController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public JewelryShopsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<JewleryShopViewModel> GetAll()
        {
            return this._context.JewelryShops.Select(js => new JewleryShopViewModel()
            {
                Id = js.JewelryShopId,
                Description = js.JewelryShopDescription,
                Name = js.JewelryShopName,
                PhoneNumbers = js.JewelryShopPhoneNumbers,
            });
        }

        [HttpGet]
        //[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public JewleryShopViewModel? GetUserActiveShop() 
        {
            var currentUserId = userManager.GetUserId(User);
            return this._context.Users.Where(u => u.Id == currentUserId)
                .Select(u => u.JewelryShop == null ? null : new JewleryShopViewModel()
                {
                    Id = u.JewelryShop.JewelryShopId,
                    Description = u.JewelryShop.JewelryShopDescription,
                    Name = u.JewelryShop.JewelryShopName,
                    PhoneNumbers = u.JewelryShop.JewelryShopPhoneNumbers,
                }).FirstOrDefault();
        }

        [HttpPost]
        //[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public async Task<JewleryShopViewModel?> SetUserActiveShop(int? id)
        {
            var currentUser = await userManager.GetUserAsync(User);
            currentUser.JewelryShopId = id;
            await userManager.UpdateAsync(currentUser);
            return this._context.Users.Where(u => u.Id == currentUser.Id)
                .Select(u => u.JewelryShop == null ? null : new JewleryShopViewModel()
                {
                    Id = u.JewelryShop.JewelryShopId,
                    Description = u.JewelryShop.JewelryShopDescription,
                    Name = u.JewelryShop.JewelryShopName,
                    PhoneNumbers = u.JewelryShop.JewelryShopPhoneNumbers,
                }).FirstOrDefault();
        }
    }
}
