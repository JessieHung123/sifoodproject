namespace sifoodproject.Areas.Stores.Models
{
    public class PutInfoModifyVM
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

        public string ClosingDay { get; set; } = null!;


        public IFormFile? LogoImageFile { get; set; }

        public IFormFile? PhotoImageFile1 { get; set; }

        public IFormFile? PhotoImageFile2 { get; set; }

        public IFormFile? PhotoImageFile3 { get; set; }


    }
}
