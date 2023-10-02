namespace BobyDesignWeb.Data.Entities
{
    public class OrderCraftingComponent
    {
        public int OrderCraftingComponentId { get; set; }

        public int WorkMaterialId { get; set; }

        public WorkMaterial WorkMaterial { get; set; }

        public decimal WorkMaterialPrice { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public decimal WorkMaterialQuantity { get; set; }
    }
}
