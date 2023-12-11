namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class HistoryOrderDetailItemVM
    {
        public string PhotoPath { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => UnitPrice * Quantity;
    }
}