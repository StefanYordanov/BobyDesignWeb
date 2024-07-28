using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using BobyDesignWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BobyDesignWeb.Controllers
{
    //[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
    public class JewelryShopsController: Controller
    {
        private readonly JewelryShopsService _jewelryShopsService;

        public JewelryShopsController(JewelryShopsService jewelryShopsService)
        {
            this._jewelryShopsService = jewelryShopsService;
        }

        [HttpGet]
        public IEnumerable<JewleryShopViewModel> GetAll()
        {
            return _jewelryShopsService.GetAll();
        }

        [HttpGet]
        public JewleryShopViewModel? GetUserActiveShop() 
        {
            return this._jewelryShopsService.GetUserActiveShop(User);
        }

        [HttpPost]
        //[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public Task<JewleryShopViewModel?> SetUserActiveShop(int? id)
        {
            return this._jewelryShopsService.SetUserActiveShop(id, User);
        }
    }
}
