using Microsoft.AspNetCore.Identity;

namespace BobyDesignWeb.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Order> Orders { get; set; }

        public JewelryShop? JewelryShop { get; set; }

        public int? JewelryShopId { get; set; }
    }
}
