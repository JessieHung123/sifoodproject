namespace sifoodproject.Areas.Stores.Models
{
    public class storeHistoryOrderDetailItemVM
    {
        public string PhotoPath { get; set; }
        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => UnitPrice * Quantity;
    }
}