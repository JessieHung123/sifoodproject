using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class CheckOutVM
    {
        public int ProductId { get; set; }

        [Range(1, 10, ErrorMessage = "{0}必須小於{1}")]
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal? TotalPrice { get; set; }

        public string? ProductName { get; set; }

        public decimal? UnitPrice { get; set; }

        public string? StoreName { get; set; }

        public List<AddressItemVM>? UserAddressList { get; set; }

        public string? UserName { get; set;}

        public string? PhotoPath { get; set; }
    }
}