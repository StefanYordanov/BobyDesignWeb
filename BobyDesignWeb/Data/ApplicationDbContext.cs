using BobyDesignWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BobyDesignWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<JewelryShop> JewelryShops { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderCraftingComponent> OrderCraftingComponents { get; set; }

        public DbSet<WorkMaterial> WorkMaterials { get; set; }

        public DbSet<WorkMaterialPriceForDate> WorkMaterialPriceForDates { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole 
            { Name = UserRolesConstants.Admin, NormalizedName = UserRolesConstants.Admin.ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole 
            { Name = UserRolesConstants.Seller, NormalizedName = UserRolesConstants.Seller.ToUpper() });
        }
    }
}