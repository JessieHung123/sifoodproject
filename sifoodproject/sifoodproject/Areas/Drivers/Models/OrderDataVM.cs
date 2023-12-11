namespace sifoodproject.Areas.Drivers.Models
{
    public class OrderDataVM
    {
        public string? OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Address { get; set; }
        public string? StoreName { get; set; }
        public string? UserName { get; set; }
        public decimal? ShippingFee { get; set; }
        public string? StatusName { get; set; }
        public int? TotalPrice { get; set; }
    }
}
