namespace BobyDesignWeb.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalOrdersCost { get; set; }
        public decimal TotalPaidCost { get; set;}
    }
}
