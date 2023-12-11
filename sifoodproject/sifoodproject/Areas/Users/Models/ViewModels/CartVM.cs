using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class CartVM 
    { 

        public string UserId { get; set; }

        public int ProductId { get; set; }

        [Range(1,10,ErrorMessage="{0}必須大於{1}")]
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString ="{0:C2}")]
        public decimal? TotalPrice { get; set; }

        public string? ProductName { get; set; } 

        public decimal? UnitPrice { get; set; }
        
        public string? StoreName { get; set; } 

        public string? PhotoPath { get; set; }

        public int RemainingStock { get; set; }
    }
}
