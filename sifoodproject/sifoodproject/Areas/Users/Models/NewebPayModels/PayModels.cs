using System.Text.Json.Serialization;

namespace sifoodproject.Areas.Users.Models.NewebPayModels
{
    public class PayModels
    {
        public class SendToNewebPayIn
        {
            [JsonPropertyName("MerchantID")]
            public string? MerchantID { get; set; }

            [JsonPropertyName("ItemDesc")]
            public string? ItemDesc { get; set; }

            [JsonPropertyName("Amt")]
            public string? Amt { get; set; }

            [JsonPropertyName("ReturnURL")]
            public string? ReturnURL { get; set; }

            [JsonPropertyName("NotifyURL")]
            public string? NotifyURL { get; set; }

            [JsonPropertyName("ClientBackURL")]
            public string? ClientBackURL { get; set; }

            public string? UserName { get; set; }

        }

        public class SendToNewebPayOut
        {
            [JsonPropertyName("MerchantID")]
            public string? MerchantID { get; set; }

            [JsonPropertyName("Version")]
            public string? Version { get; set; }

            [JsonPropertyName("TradeInfo")]
            public string? TradeInfo { get; set; }

            [JsonPropertyName("TradeSha")]
            public string? TradeSha { get; set; }
        }
    }

}
