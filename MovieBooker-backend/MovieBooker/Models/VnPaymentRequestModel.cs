namespace MovieBooker.Models
{
    public class VnPaymentRequestModel
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set;}
        public int OrderId { get; set; }
    }
}
