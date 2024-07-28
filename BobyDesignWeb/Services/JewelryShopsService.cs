using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Data;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BobyDesignWeb.Services
{
    public class JewelryShopsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public JewelryShopsService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

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
        public JewleryShopViewModel? GetUserActiveShop(ClaimsPrincipal user)
        {
            var currentUserId = userManager.GetUserId(user);
            return this._context.Users.Where(u => u.Id == currentUserId)
                .Select(u => u.JewelryShop == null ? null : new JewleryShopViewModel()
                {
                    Id = u.JewelryShop.JewelryShopId,
                    Description = u.JewelryShop.JewelryShopDescription,
                    Name = u.JewelryShop.JewelryShopName,
                    PhoneNumbers = u.JewelryShop.JewelryShopPhoneNumbers,
                }).FirstOrDefault();
        }

        public async Task<JewleryShopViewModel?> SetUserActiveShop(int? id, ClaimsPrincipal user)
        {
            var currentUser = await userManager.GetUserAsync(user);
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

