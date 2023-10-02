using BobyDesignWeb.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BobyDesignWeb.Data
{
    public class ApplicationDbSeeder
    {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationDbSeeder(ApplicationDbContext ctx, UserManager<ApplicationUser> userManager)
        {
            this._ctx = ctx;
            this._userManager = userManager;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();
            ApplicationUser mainAdmin = await _userManager.FindByEmailAsync("stefanyordanov324@gmail.com");
            if (mainAdmin == null)
            {
                mainAdmin = new()
                {
                    UserName = "stefanyordanov324@gmail.com",
                    Email = "stefanyordanov324@gmail.com",
                    FirstName = "Stefan",
                    LastName = "Yordanov",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(mainAdmin, "SteepHurriedCosmetics1!");
            }

            if ((await _userManager.GetUsersInRoleAsync(UserRolesConstants.Admin)).Count == 0)
            {
               await _userManager.AddToRoleAsync(mainAdmin, UserRolesConstants.Admin);
            }
        }
    }
}
