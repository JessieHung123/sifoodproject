namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class CreateTakeOutOrderVM
    {
        public string? StoreName { get; set; }
        public string? UserName { get; set; }
        public List<OrderItemVM> ProductDetails { get; set; }
        public int TotalPrice { get; set; }
    }
}
