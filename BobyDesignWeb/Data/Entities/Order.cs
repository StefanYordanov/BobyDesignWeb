using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BobyDesignWeb.Data.Entities
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public int JewelryShopId { get; set; }

        public JewelryShop JewelryShop { get; set; }

        [MaxLength(500)]
        public string OrderDescription { get; set; }

        public ICollection<OrderCraftingComponent> OrderCraftingComponents { get; set; }

        public DateTime OrderCreatedOn { get; set; }

        [Column(TypeName = "Date")]
        public DateTime FinishingDate { get; set; }

        public decimal LaborPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal Deposit { get; set; }

        public string ImageFileName {  get; set; }

        public string ShopUserId { get; set; }

        public ApplicationUser ShopUser { get; set; }

        public bool NotifyCustomer { get; set; }

        public OrderStatus Status { get; set; }

        public StoredFile? StoredFile { get; set; }

        public int? StoredFileId { get; set; }
    }
}
