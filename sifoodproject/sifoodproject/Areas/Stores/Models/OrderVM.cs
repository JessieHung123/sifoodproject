namespace sifoodproject.Areas.Stores.Models
{
    public class OrderVM
    {
        public string OrderId { get; set; }

        public double OrderDuration { get; set; }

        public DateTime OrderDateTime { get; set; }

        public string OrderDate { get; set; }

        public string OrderTime { get; set; }

        public string DeliveryMethod { get; set; }

        public string Address { get; set; } = null!;

        public string Status { get; set; }

        public int StatusId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; } = null!;

        public string UserPhone { get; set; } = null!;

        public string? PaymentMethodＮame { get; set; }

        //public DateTime PaymentTime { get; set; }

        public string PaymentDate { get; set; }

        public string PaymentTime { get; set; }

        public IEnumerable<OrderDetailsVM> OrderDetails { get; set; }

        public string DriverFullName { get; set; }

        public decimal? ShippingFee { get; set; }

        public decimal Subtotal { get; set; }

        public int TotalQuantity { get; set; }
    }
}
