namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class StoreProductsVM
    {
        public string StoreId { get; set; } = null!;

        public string StoreName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string OpeningTime { get; set; } = null!;

        public string? PhotosPath { get; set; }

        public string? LogoPath { get; set; }

        public int? CommentCount { get; set; }

        public int? CommentRank { get; set; }

        public string? WeekdayOpeningTime { get; set; }

        public string? WeekendOpeningTime { get; set; }

        public bool OpenForBusiness { get; set; }

        public Array? CategoryList { get; set; }

        public IEnumerable<ProductsVM> Products { get; set; }

        public IEnumerable<CommentVM> Comment { get; set; }

    }
}
