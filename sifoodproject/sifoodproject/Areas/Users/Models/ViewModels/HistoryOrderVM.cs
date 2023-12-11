namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class HistoryOrderVM
    {
        public string StoreId { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }

        public string? PhotoPath { get; set; }

        public string ProductName { get; set; } = null!;

        //第一個商品照片&名稱
        public string FirstProductPhotoPath { get; set; }
        public string FirstProductName { get; set; }

        //加入OrderDetail
        public HistoryOrderDetailVM OrderDetails { get; set; }

    }
}