namespace sifoodproject.Areas.Admin.Models
{
    public class UserAddressViewModel
    {
        public int UserAddressId { get; set; }

        public string UserId { get; set; } = null!;

        public string UserCity { get; set; } = null!;

        public string UserRegion { get; set; } = null!;

        public string UserDetailAddress { get; set; } = null!;

        public decimal UserLatitude { get; set; }

        public decimal UserLongitude { get; set; }

        public bool IsDefault { get; set; }
    }
}
