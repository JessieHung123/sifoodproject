
namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class FavoriteVM
    {
        public string UserId { get; set; } = null!;

        public string StoreId { get; set; } = null!;

        public int FavoriteId { get; set; }
        public string StoreName { get; set; }
        public string LogoPath { get; set; }
    }
}