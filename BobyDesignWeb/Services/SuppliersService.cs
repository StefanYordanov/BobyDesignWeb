using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Data;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BobyDesignWeb.Services
{
    public class SuppliersService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public SuppliersService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

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
    }
}
