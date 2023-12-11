using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }

        public string StoreName { get; set; } = null!;
        public string StoreId { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? Description { get; set; } 

        public string? PhotoPath { get; set; }

        public string SuggestPickUpTime { get; set; } = null!;

        public string SuggestPickEndTime { get; set; } = null!;

        public int ReleasedQty { get; set; }

        public int OrderedQty { get; set; }

    }
}
