using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class JoinUsVM
    {

        [Required(ErrorMessage = "店名未填寫")]
        [Display(Name = "平台顯示店名")]
        public string StoreName { get; set; }


        [Required(ErrorMessage = "聯絡人姓名未填寫")]
        [Display(Name = "聯絡人姓名")]
        public string ContactName { get; set; }

        [EmailAddress(ErrorMessage = "Email格式錯誤")]
        [Required(ErrorMessage = "Email未填寫")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "聯絡電話未填寫")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "無效的手機號碼格式")]
        [Display(Name = "聯絡電話")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "公司統編未填寫")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "無效的公司統編格式")]
        [Display(Name = "公司統編")]
        public string TaxId { get; set; }

        [Required(ErrorMessage = "店家所在城市未填寫")]
        [RegularExpression("新北市|臺北市", ErrorMessage = "城市必須是新北市或臺北市")]
        [Display(Name = "店家地址-市")]
        public string City { get; set; }

        [Required(ErrorMessage = "店家所在區未填寫")]
        [Display(Name = "店家地址-區")]
        public string Region { get; set; }

        [Required(ErrorMessage = "店家地址未填寫")]
        [Display(Name = "店家地址")]
        public string Address { get; set; }
        [MaxLength(25, ErrorMessage = "描述不得多於25個字元")]
        [Required(ErrorMessage = "店家介紹未填寫")]
        [Display(Name = "店家介紹")]
        public string Description { get; set; }

        [Required(ErrorMessage = "公休日未填寫")]
        [Display(Name = "公休日")]
        public string ClosingDay { get; set; }

        [Required(ErrorMessage = "請上傳LOGO")]
        [Display(Name = "LOGO")]
        //改類型為IFormFile
        public IFormFile LogoPath { get; set; }

        [Required(ErrorMessage = "請上傳店家照片")]
        [Display(Name = "店家照片")]
        //改類型,用於處理多個文件
        //然後，在控制器中，會遍歷這個列表並分別處理每個文件
        public IFormFile PhotosPath { get; set; }


        public IFormFile PhotosPath2 { get; set; }
        public IFormFile PhotosPath3 { get; set; }

        [Required(ErrorMessage = "請填寫週間開始時間")]
        [Display(Name = "週間開始時間")]
        public string WeekdayStartTime { get; set; }

        [Required(ErrorMessage = "請填寫週間結束時間")]
        [Display(Name = "週間結束時間")]
        public string WeekdayEndTime { get; set; }

        [Required(ErrorMessage = "請填寫週末開始時間")]
        [Display(Name = "週末開始時間")]
        public string WeekendStartTime { get; set; }

        [Required(ErrorMessage = "請填寫週末結束時間")]
        [Display(Name = "週末結束時間")]
        public string WeekendEndTime { get; set; }

        [Required(ErrorMessage = "經緯度未填寫")]
        public decimal Latitude { get; set; }
        [Required(ErrorMessage = "經緯度未填寫")]
        public decimal Longitude { get; set; }


    }
}