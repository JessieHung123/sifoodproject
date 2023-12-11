namespace sifoodproject.Areas.Stores.Models
{
    public class ReviewVM
    {
        public string UserId { get; set; } = null!;

        public string OrderId { get; set; } = null!;

        public short CommentRank { get; set; }

        public DateTime CommentTime { get; set; }

        public string? Contents { get; set; }

        public string StoreId { get; set; } = null!;

        public string? UserName { get; set; }
    }
}
