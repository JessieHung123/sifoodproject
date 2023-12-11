using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Admin.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "請輸入登入帳號")]
        public string? Account { get; set; }

        [Required(ErrorMessage = "請輸入登入密碼")]
        public string? Password { get; set; }
    }
}
