namespace BobyDesignWeb.Models.Reports
{
    public class OrderItemReportItem : OrderCraftingComponentViewModel
    {
        public OrderReportItem Order {  get; set; } = new();
    }

    public class OrderReportItem
    {
        public int Id { get; set; }

        public DateTime OrderCreatedOn { get; set; }
    }

    public class OrderItemsReport
    {
        public ICollection<OrderItemReportItem> Items { get; set; } = Array.Empty<OrderItemReportItem>();

        public decimal TotalQuantity { get; set; }

        public decimal TotalRevenue {  get; set; }
    }
}
