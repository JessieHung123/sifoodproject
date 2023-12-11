namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class CreateDeliverOrderVM
    {
        public string? StoreName { get; set; }
        public string? UserName { get; set; }
        public List<OrderItemVM> ProductDetails { get; set; }
        public int TotalPrice { get; set; }
        public string? UserAddress { get; set; }
        public int ShippingFee { get; set; }
    }
}
