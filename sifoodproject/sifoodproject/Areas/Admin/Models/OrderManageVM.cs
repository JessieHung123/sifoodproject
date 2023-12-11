namespace sifoodproject.Areas.Admin.Models
{
    public class OrderManageVM
    {
        public string? OrderAddress { get; set; }
        public string? OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public int Quantity { get; set; }
        public string? UserPhone { get; set; }
        public int OrderDetailId { get; set; }
        public ICollection<UserAddressViewModel>? UserAddress { get; internal set; }
        public DateTime OrderDate { get; set; }
        public string? StatusName { get; internal set; }
        public decimal ProductUnitPrice { get; set; }
        public string? StoreName { get; set; }
        public string? StorePhone { get; set; }
        public string? StoreAddress { get; set; }
        public decimal Total { get; set; }
    }
}
