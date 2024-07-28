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
    public class SuppliersController: Controller
    {
        private readonly SuppliersService _suppliersService;

        public SuppliersController(SuppliersService suppliersService)
        {
            this._suppliersService = suppliersService;
        }

        [HttpGet]
        public IEnumerable<SupplierViewModel> GetAll()
        {
            return _suppliersService.GetAll();
        }

        [HttpGet]
        //[Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
        public SupplierViewModel? GetDefaultActiveSupplier() 
        {
            return _suppliersService.GetDefaultActiveSupplier();
        }
    }
}
