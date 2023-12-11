namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class HistoryOrderDetailVM
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? ShippingFee { get; set; }

        //新增訂購資訊
        public string DeliveryMethod { get; set; } = null!;

        public List<HistoryOrderDetailItemVM> Items { get; set; }
        public int? CommentRank { get; set; }
        public string? CommentContents { get; set; }

        // 新增計算屬性以計算所有商品的總小計
        public decimal ItemsSubTotal
        {
            get
            {
                return Items.Sum(item => item.SubTotal);
            }
        }

        // TotalPrice 現在可以是 ItemsSubTotal 加上 ShippingFee
        public decimal TotalPrice
        {
            get
            {
                return ItemsSubTotal + (ShippingFee ?? 0);
            }
        }
    }
}