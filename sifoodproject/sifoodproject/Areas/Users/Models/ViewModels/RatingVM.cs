namespace sifoodproject.Areas.Users.Models.ViewModels
{
    
    public class RatingVM
    {
        public string OrderId { get; set; } // 訂單ID
        public int Rating { get; set; } // 評分數值
        public string Comment { get; set; } // 評論內容 
    }

}
