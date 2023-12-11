namespace sifoodproject.Areas.Drivers.Models
{
    public class ChooseOrderVM
    {
        public string OrderId { get; set; } = null!;
        public string? Address { get; set; }
        public string? StoreAddress { get; set; }
        public string StoreId { get; set; } = null!;

        public string StoreName { get; set; } = null!;

        public string? DriverId { get; set; }

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public string UserPhone { get; set; } = null!;

        public int StatusId { get; set; }

        public IEnumerable<OrderDetailsVM> OrderDetails { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
