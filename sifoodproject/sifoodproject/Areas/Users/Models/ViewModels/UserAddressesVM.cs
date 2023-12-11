namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class UserAddressesVM
    {
        public int UserAddressId { get; set; }

        public string UserCity { get; set; } = null!;

        public string? UserId { get; set; }

        public string? UserName { get; set; } 

        public string UserRegion { get; set; } = null!;

        public string UserDetailAddress { get; set; } = null!;

        public string? UserPhone { get; set; }

        public bool IsDefault { get; set; }
    }
}
