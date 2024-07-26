using BobyDesignWeb.Data.Entities;

namespace BobyDesignWeb.Models
{
    public class CustomerReviewViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public CustomerReviewType Type { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CustomerId { get; set; }
    }
}
