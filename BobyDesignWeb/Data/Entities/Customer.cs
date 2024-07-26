using System.ComponentModel.DataAnnotations;

namespace BobyDesignWeb.Data.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public ICollection<Order> Orders { get; set; }

        [MaxLength(50)]
        public string CustomerName { get; set; }

        [MaxLength(50)]
        public string CustomerEmail { get; set; }
        [MaxLength(20)]
        public string CustomerPhone { get; set; }

        public ICollection<CustomerReview> CustomerReviews { get; set; }

    }
}
