using BobyDesignWeb.Data.Entities;

namespace BobyDesignWeb.Models
{
    public class SubmitEditOrderModel
    {
        public bool UpdateImage { get; set; }
        public IEnumerable<int> DeletedCraftingComponentIds { get; set; } = new List<int>();
        public OrderViewModel Model { get; set; }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public CustomerViewModel Customer { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public OrderPaymentMethod PaymentMethod { get; set; }

        public JewleryShopViewModel Shop { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public DateOnlyModel FinishingDate { get; set; }
        public ICollection<OrderCraftingComponentViewModel> CraftingComponents { get; set; } = new List<OrderCraftingComponentViewModel>();
        public decimal LaborPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Deposit { get; set; }
        public OrderStatus Status { get; set; }
        public int? LinkedOrderId { get; set; }
        public OrderType Type { get; set; }
        public string? ImageFileName { get; set; }
        public UserViewModel ShopUser { get; set; } = new UserViewModel();
    }

    public class PayOrderQuery
    {
        public int OrderId { get; set; }

        public decimal Payment { get; set; }
    }

    public class OrderCraftingComponentViewModel
    {
        public int Id { get; set; }
        public WorkMaterialModel WorkMaterial {  get; set; }

        public bool IsDeposit { get; set; }
        public decimal WorkMaterialPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalComponentPrice {  get; set; }
    }

    public enum OrderStatus
    {
        Opened = 1,
        Closed = 2
    }
}
