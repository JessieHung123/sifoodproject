namespace sifoodproject.Areas.Stores.Models
{
    public class ProductManageVM
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string StoreId { get; set; } = null!;

        public int CategoryId { get; set; }

        public int ReleasedQty { get; set; }

        public int OrderedQty { get; set; }

        public string? PhotoPath { get; set; }

        public string? Description { get; set; }

        public string RealeasedTime { get; set; }

        public TimeSpan SuggestPickUpTime { get; set; }

        public TimeSpan SuggestPickEndTime { get; set; }

        public decimal UnitPrice { get; set; }

        public string CategoryName { get; set; } = null!;


    }
}
