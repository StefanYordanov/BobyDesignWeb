namespace BobyDesignWeb.Data.Entities
{
    public class CustomerReview
    {
        public int CustomerReviewId { get; set; }

        public string CustomerReviewText { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public CustomerReviewType CustomerReviewType { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
    }

    public enum CustomerReviewType
    {
        Positive=1, Neutral = 2, Negative = 3, 
    }
}
