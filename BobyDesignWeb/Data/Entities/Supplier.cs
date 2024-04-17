using System.ComponentModel.DataAnnotations;

namespace BobyDesignWeb.Data.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [MaxLength(50)]
        public string SupplierName { get; set; }

        [MaxLength(1000)]
        public string SupplierDescription { get; set; }

        [MaxLength(100)]
        public string SupplierPhoneNumbers { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
