using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Data;
using BobyDesignWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BobyDesignWeb.Utils
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddTransient<ApplicationDbSeeder>();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<CustomersService>();
            builder.Services.AddTransient<JewelryShopsService>();
            builder.Services.AddTransient<OrdersService>();
            builder.Services.AddTransient<PaginationService>();
            builder.Services.AddTransient<ReportsService>();
            builder.Services.AddTransient<StoredFilesService>();
            builder.Services.AddTransient<SuppliersService>();
            builder.Services.AddTransient<UsersService>();
            builder.Services.AddTransient<WebContentService>();
            builder.Services.AddTransient<WorkMaterialService>();

            return builder;
        }
    }
}
