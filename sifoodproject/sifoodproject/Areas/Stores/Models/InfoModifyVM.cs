namespace sifoodproject.Areas.Stores.Models
{
    public class InfoModifyVM
    {
        public string StoreId { get; set; } = null!;

        public string StoreName { get; set; } = null!;

        public string ContactName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string OpeningTime { get; set; } = null!;

        public string WeekdayOpen { get; set; } = null!;

        public string WeekdayClose { get; set; } = null!;

        public string WeekendOpen { get; set; } = null!;

        public string WeekendClose { get; set; } = null!;

        public string ClosingDay { get; set; } = null!;

        public string? PhotosPath { get; set; }

        public string? LogoPath { get; set; }

        public string? PhotosPath2 { get; set; }

        public string? PhotosPath3 { get; set; }

    }
}
