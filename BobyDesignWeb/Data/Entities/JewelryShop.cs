using System.ComponentModel.DataAnnotations;

namespace BobyDesignWeb.Data.Entities
{
    public class JewelryShop
    {
        public int JewelryShopId { get; set; }

        [MaxLength(50)]
        public string JewelryShopName { get; set; }

        [MaxLength(1000)]
        public string JewelryShopDescription { get; set; }

        [MaxLength(100)]
        public string JewelryShopPhoneNumbers { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
