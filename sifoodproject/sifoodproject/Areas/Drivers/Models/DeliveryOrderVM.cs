namespace sifoodproject.Areas.Drivers.Models
{
    public class DeliveryOrderVM
    {
        public string OrderId { get; set; } = null!;
        public string? Address { get; set; }
        public string? StoreAddress { get; set; }
        public string StoreName { get; set; } = null!;
        public string? DriverId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public int StatusId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? OrderDate { get; set; }
        public string? OrderTime { get; set; }

    }
}
