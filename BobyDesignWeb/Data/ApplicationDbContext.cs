using BobyDesignWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

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

        public DbSet<StoredFile> StoredFiles { get; set; }

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
            { Name = UserRolesConstants.Admin, NormalizedName = UserRolesConstants.Admin.ToUpper(),
                Id= "e2875763-6213-4636-8ab1-38d1b687abb2", ConcurrencyStamp= "ec92d97c-486a-475c-8e12-11e26ff7b794"
            });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole 
            { Name = UserRolesConstants.Seller, NormalizedName = UserRolesConstants.Seller.ToUpper(), 
                Id= "2b89a0db-4bbc-4a80-9b30-d3df652ca902", ConcurrencyStamp= "56e1e0d0-79b8-4c15-b55-bcbdaecbf0d0"
            });
        }
    }
}