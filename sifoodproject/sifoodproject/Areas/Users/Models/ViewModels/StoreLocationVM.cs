using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class StoreLocationVM
    {
        
        public string StoreId { get; set; } = null!;

        public string StoreName { get; set; } = null!;

        public string Description { get; set; } = null!;
        public string? LogoPath { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? CommentRank { get; set; }

        public int CommentCount { get; set; }

        public string City { get; set; } = null!;

        public string Region { get; set; } = null!;

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public string? PhotosPath { get; set; }

        public string? PhotosPath2 { get; set; }

        public string? PhotosPath3 { get; set; }

        public string Address { get; set; } = null!;

        public string? WeekdayOpeningTime { get; set; }
        public string? WeekendOpeningTime { get; set; }
        public string ClosingDay { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public IEnumerable<string>? CategoryList { get; set; }
    }
}
