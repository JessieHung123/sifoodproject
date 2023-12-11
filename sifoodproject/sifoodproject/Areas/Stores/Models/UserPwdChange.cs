namespace sifoodproject.Areas.Stores.Models
{
    public class UserPwdChange
    {
        public string? UserEmail { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
