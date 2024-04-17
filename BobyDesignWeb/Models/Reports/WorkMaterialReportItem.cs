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

        public decimal TotalSoldQuantity { get; set; }

        public decimal TotalSoldRevenue { get; set; }

        public decimal TotalDepositQuantity { get; set; }

        public decimal TotalDepositRevenue { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalRevenue {  get; set; }
    }
}
