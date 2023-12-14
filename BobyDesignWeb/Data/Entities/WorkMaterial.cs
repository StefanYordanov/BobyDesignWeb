using System.ComponentModel.DataAnnotations;

namespace BobyDesignWeb.Data.Entities
{
    public class WorkMaterial
    {
        public int WorkMaterialId { get; set; }

        [MaxLength(50)]
        public string WorkMaterialName { get; set; }

        [MaxLength(50)]
        public string WorkMaterialMeasuringUnit { get; set; }

        public MaterialPricingType WorkMaterialPricingType { get; set; }

        public ICollection<WorkMaterialPriceForDate> WorkMaterialPriceForDates { get; set; }

        public ICollection<OrderCraftingComponent> OrderCraftingComponents { get; set; }


    }
}
