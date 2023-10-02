using Microsoft.EntityFrameworkCore;

namespace BobyDesignWeb.Data.Entities
{
    [Index(nameof(Date))]
    public class WorkMaterialPriceForDate
    {
        public int WorkMaterialPriceForDateId { get; set; }

        public int WorkMaterialId { get; set; }

        public WorkMaterial WorkMaterial { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal PurchasingPrice { get; set; }

        public DateTime Date { get; set; }
    }
}
