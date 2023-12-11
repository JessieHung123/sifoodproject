namespace sifoodproject.Areas.Drivers.Models
{
    public class OrderDetailsVM
    {
        public string PhotoPath { get; set; } = null!;

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }
    }
}
