using BobyDesignWeb.Data.Entities;

namespace BobyDesignWeb.Models
{
    public class LatestWorkMaterialRelevantPriceModel
    {
        public int Id { get; set; }

        public int WorkMaterialId { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal PurchasingPrice { get; set; }

        public DateTime LastUpdatedOn { get; set; }
    }

    public class WorkMaterialModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string MeasuringUnit { get; set; } = string.Empty;

        public MaterialPricingType PricingType { get; set; }

        public LatestWorkMaterialRelevantPriceModel? RelevantPrice { get; set; }
    }
}
