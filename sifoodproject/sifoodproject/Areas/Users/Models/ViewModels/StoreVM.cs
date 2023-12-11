using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class StoreVM
    {
        [Key]
        public string StoreId { get; set; } = null!;

        public string StoreName { get; set; } = null!;

        public string Description { get; set; } = null!;
        public string? LogoPath { get; set; } = null!;
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? CommentRank { get; set; }

        public int CommentCount { get; set; } 

        public IEnumerable<string> CategoryName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string WeekdayOpeningTime { get; set; } = null!;
        public string WeekdayClosingTime { get; set; } = null!;

        public string WeekendOpeningTime { get; set; } = null!;

        public string WeekendClosingTime { get; set; } = null!;
        public int? Inventory { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
