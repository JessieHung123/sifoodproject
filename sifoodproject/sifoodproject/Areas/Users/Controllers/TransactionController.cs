using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Users.Models.NewebPayModels;
using sifoodproject.Areas.Users.Models.ViewModels;
using sifoodproject.Models;
using sifoodproject.Services;
using System.Diagnostics;
using System.Text;
using static sifoodproject.Areas.Users.Models.NewebPayModels.PayModels;

namespace sifoodproject.Areas.Users.Controllers
{
    [Area("Users")]
    public class TransactionController : Controller
    {
        private readonly Sifood3Context _context;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IConfiguration _configuration;
        public TransactionController(Sifood3Context context, IUserIdentityService userIdentityService, IConfiguration configuration)
        {
            _userIdentityService = userIdentityService;
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        public IActionResult CheckOut()
        {
            ViewData["MerchantID"] = _configuration.GetSection("MerchantID").Value;
            ViewData["ReturnURL"] = $"https://sifood103.azurewebsites.net/Users/Transaction/CallbackReturn";
            ViewData["NotifyURL"] = $"https://sifood103.azurewebsites.net/Users/Transaction/CallbackNotify";
            ViewData["ClientBackURL"] = $"https://sifood103.azurewebsites.net/Users/Home/Main";
            return View();
        }

        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [HttpGet]
        [Route("/Transaction/GetCheckoutData")]
        public IQueryable<CheckOutVM> GetCheckoutData()
        {
            string userId = _userIdentityService.GetUserId();
            var CheckOutData = _context.Carts.Include(x => x.User).Where(y => y.UserId == userId && y.Product.IsDelete == 1 &&
            y.Product.RealeasedTime.Date == DateTime.Now.Date && y.Product.SuggestPickEndTime > DateTime.Now.TimeOfDay 
            && y.Product.ReleasedQty - y.Product.OrderedQty !=0)
            .Select(y => new CheckOutVM
            {
                UserName = y.User.UserName,
                UserAddressList = (List<AddressItemVM>)y.User.UserAddresses.Select(x => new AddressItemVM
                {
                    UserAddress = x.UserDetailAddress,
                    AdressIsDefault = x.IsDefault,
                }),
                ProductId = y.ProductId,
                ProductName = _context.Products.Where(c => c.ProductId == y.ProductId).Select(x => x.ProductName).Single(),
                Quantity = y.Quantity,
                UnitPrice = _context.Products.Where(c => c.ProductId == y.ProductId).Select(x => x.UnitPrice).FirstOrDefault(),
                TotalPrice = y.Quantity * _context.Products.Where(c => c.ProductId == y.ProductId).Select(x => x.UnitPrice).FirstOrDefault(),
                StoreName = _context.Stores.Where(s => s.StoreId == y.Product.StoreId).Select(p => p.StoreName).Single(),
                PhotoPath = _context.Products.Where(p => p.ProductId == y.ProductId).Select(c => c.PhotoPath).Single(),
            });
            return CheckOutData;
        }

        [HttpPost]
        [Route("/Transaction/TakeOutOrder")]
        public string TakeOutOrder([FromBody] CreateTakeOutOrderVM model)
        {
            string StoreId = _context.Stores.Where(s => s.StoreName == model.StoreName).Select(s => s.StoreId).Single();
            string UserId = _context.Users.Where(x => x.UserName == model.UserName).Select(x => x.UserId).Single();
            User? user = _context.Users.FirstOrDefault(x => x.UserName == model.UserName);
            user.TotalOrderAmount += model.TotalPrice;
            TimeZoneInfo taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, taiwanTimeZone);
            Order order = new()
            {
                OrderDate = taiwanTime,
                StoreId = StoreId,
                UserId = UserId,
                DeliveryMethod = "自取",
                StatusId = 1,
                TotalPrice = model.TotalPrice
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            foreach (var items in model.ProductDetails)
            {
                int productId = GetProductIdByName(items.ProductName, items.StoreName);
                OrderDetail orderDetail = new()
                {
                    OrderId = order.OrderId,
                    ProductId = productId,
                    Quantity = items.Quantity,
                };
                _context.OrderDetails.Add(orderDetail);
                Product? product = _context.Products.Where(x => x.ProductId == productId).FirstOrDefault();
                int orderedQty = _context.Products.Where(x => x.ProductId == productId).Select(y => y.OrderedQty).Single();
                product.OrderedQty = orderedQty + items.Quantity;
            }
            Payment payment = new()
            {
                OrderId = order.OrderId,
                PaymentMethodＮame = "藍新",
                PaymentStatusＮame = "未付款",
                PaymentTime = DateTime.Now,
            };
            List<Cart> cartItems = _context.Carts.Where(c => c.UserId == UserId).ToList();
            _context.Carts.RemoveRange(cartItems);
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return "訂單下訂成功";
        }

        private int GetProductIdByName(string productName, string storeName)
        {
            var storeId = _context.Stores.Where(x => x.StoreName == storeName).Select(y => y.StoreId).FirstOrDefault();
            return _context.Products.Where(p => p.ProductName == productName && p.StoreId == storeId && p.IsDelete == 1).Select(p => p.ProductId).FirstOrDefault();
        }

        [HttpPost]
        [Route("/Transaction/DeliverOrder")]
        public string DeliverOrder([FromBody] CreateDeliverOrderVM model)
        {
            string StoreId = _context.Stores.Where(s => s.StoreName == model.StoreName).Select(s => s.StoreId).Single();
            string UserId = _context.Users.Where(x => x.UserName == model.UserName).Select(x => x.UserId).Single();
            User? user = _context.Users.FirstOrDefault(x => x.UserName == model.UserName);
            user.TotalOrderAmount += model.TotalPrice;
            TimeZoneInfo taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, taiwanTimeZone);
            Order order = new()
            {
                OrderDate = taiwanTime,
                StoreId = StoreId,
                UserId = UserId,
                DeliveryMethod = "外送",
                StatusId = 1,
                TotalPrice = model.TotalPrice,
                ShippingFee = 40,
                Address = model.UserAddress
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            foreach (var item in model.ProductDetails)
            {
                int productId = GetProductIdByName(item.ProductName, item.StoreName);
                OrderDetail orderDetail = new()
                {
                    OrderId = order.OrderId,
                    ProductId = productId,
                    Quantity = item.Quantity,
                };
                _context.OrderDetails.Add(orderDetail);
                Product? product = _context.Products.Where(x => x.ProductId == productId).FirstOrDefault();
                int orderedQty = _context.Products.Where(x => x.ProductId == productId).Select(y => y.OrderedQty).Single();
                product.OrderedQty = orderedQty + item.Quantity;
            }
            Payment payment = new()
            {
                OrderId = order.OrderId,
                PaymentMethodＮame = "藍新",
                PaymentStatusＮame = "未付款",
                PaymentTime = DateTime.Now,
            };
            List<Cart> cartItems = _context.Carts.Where(c => c.UserId == UserId).ToList();
            _context.Carts.RemoveRange(cartItems);
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return "訂單下訂成功!";
        }

        /// <summary>
        /// 傳送訂單至藍新金流
        /// </summary>
        [HttpPost]
        [Route("/Transaction/SendToNewebPay")]
        public JsonResult SendToNewebPay([FromForm] SendToNewebPayIn inModel)
        {
            string orderId = _context.Orders.Include(o => o.User).Where(o => o.User.UserName == inModel.UserName).OrderBy(o => o.OrderId).Select(o => o.OrderId).LastOrDefault().ToString();
            SendToNewebPayOut outModel = new()
            {
                MerchantID = inModel.MerchantID,
                Version = "2.0"
            };
            List<KeyValuePair<string, string>> TradeInfo = new()
            {
            new("MerchantID", inModel.MerchantID),
            new("RespondType", "String"),
            new("TimeStamp", DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString()),
            new("Version", "2.0"),
            new("MerchantOrderNo", orderId),
            new("Amt", inModel.Amt),
            new("ItemDesc", inModel.ItemDesc),
            new("ReturnURL", inModel.ReturnURL),
            new("NotifyURL", inModel.NotifyURL),
            new("ClientBackURL", inModel.ClientBackURL),
            new("EmailModify", "1")
            };
            string TradeInfoParam = string.Join("&", TradeInfo.Select(x => $"{x.Key}={x.Value}"));
            string HashKey = _configuration.GetSection("HashKey").Value;
            string HashIV = _configuration.GetSection("HashIV").Value;
            string TradeInfoEncrypt = EncryptAESHex(TradeInfoParam, HashKey, HashIV);
            outModel.TradeInfo = TradeInfoEncrypt;
            outModel.TradeSha = EncryptSHA256($"HashKey={HashKey}&{TradeInfoEncrypt}&HashIV={HashIV}");
            return Json(outModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorPayViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 支付完成返回網址
        /// </summary>
        public IActionResult CallbackReturn(CallBackVM model)
        {
            string hashKey = _configuration.GetSection("HashKey").Value;
            string hashIV = _configuration.GetSection("HashIV").Value;
            string decryptedTradeInfo = DecryptAESHex(model.TradeInfo, hashKey, hashIV);
            var keyValuePairs = decryptedTradeInfo.Split('&').Select(part => part.Split('=')).ToDictionary(split => split[0], split => split[1]);
            model.Status = keyValuePairs["Status"];
            model.MerchantOrderNo = keyValuePairs["MerchantOrderNo"];
            Payment? payment = _context.Payments.Where(x => x.OrderId == model.MerchantOrderNo).FirstOrDefault();
            if (model.Status == "SUCCESS")
            {
                payment.PaymentStatusＮame = "已付款";
                _context.SaveChanges();
                return RedirectToAction("RealTimeOrders", "Home");
            }
            else
            {
                return RedirectToAction("PaymentFail", "Transaction");
            }
        }

        /// <summary>
        /// 支付通知網址
        /// </summary>
        public IActionResult CallbackNotify(CallBackVM model)
        {
            string hashKey = _configuration.GetSection("HashKey").Value;
            string hashIV = _configuration.GetSection("HashIV").Value;
            string decryptedTradeInfo = DecryptAESHex(model.TradeInfo, hashKey, hashIV);
            var keyValuePairs = decryptedTradeInfo.Split('&').Select(part => part.Split('=')).ToDictionary(split => split[0], split => split[1]);
            model.Status = keyValuePairs["Status"];
            model.MerchantOrderNo = keyValuePairs["MerchantOrderNo"];
            Payment? payment = _context.Payments.Where(x => x.OrderId == model.MerchantOrderNo).FirstOrDefault();
            if (model.Status == "SUCCESS")
            {
                payment.PaymentStatusＮame = "已付款";
                _context.SaveChanges();
                return RedirectToAction("RealTimeOrders", "Home");
            }
            else
            {
                return RedirectToAction("PaymentFail", "Transaction");
            }
        }

        /// <summary>
        /// 加密後再轉 16 進制字串
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        public string EncryptAESHex(string source, string cryptoKey, string cryptoIV)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(source))
            {
                var encryptValue = EncryptAES(Encoding.UTF8.GetBytes(source), cryptoKey, cryptoIV);
                if (encryptValue != null)
                {
                    result = BitConverter.ToString(encryptValue)?.Replace("-", string.Empty)?.ToLower();
                }
            }
            return result;
        }

        /// <summary>
        /// 字串加密AES
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        public byte[] EncryptAES(byte[] source, string cryptoKey, string cryptoIV)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);
            using var aes = System.Security.Cryptography.Aes.Create();
            aes.Mode = System.Security.Cryptography.CipherMode.CBC;
            aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            aes.Key = dataKey;
            aes.IV = dataIV;
            using var encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(source, 0, source.Length);
        }

        /// <summary>
        /// 字串加密SHA256
        /// </summary>
        /// <param name="source">加密前字串</param>
        public string EncryptSHA256(string source)
        {
            string result = string.Empty;
            using (System.Security.Cryptography.SHA256 algorithm = System.Security.Cryptography.SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(source));
                if (hash != null)
                {
                    result = BitConverter.ToString(hash)?.Replace("-", string.Empty)?.ToUpper();
                }
            }
            return result;
        }

        /// <summary>
        /// 16 進制字串解密
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        public string DecryptAESHex(string source, string cryptoKey, string cryptoIV)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(source))
            {
                byte[] sourceBytes = ToByteArray(source);
                if (sourceBytes != null)
                {
                    result = Encoding.UTF8.GetString(DecryptAES(sourceBytes, cryptoKey, cryptoIV)).Trim();
                }
            }
            return result;
        }

        /// <summary>
        /// 將16進位字串轉換為byteArray
        /// </summary>
        /// <param name="source">欲轉換之字串</param>
        public byte[] ToByteArray(string source)
        {
            byte[] result = null;
            if (!string.IsNullOrWhiteSpace(source))
            {
                var outputLength = source.Length / 2;
                var output = new byte[outputLength];
                for (var i = 0; i < outputLength; i++)
                {
                    output[i] = Convert.ToByte(source.Substring(i * 2, 2), 16);
                }
                result = output;
            }
            return result;
        }

        /// <summary>
        /// 字串解密AES
        /// </summary>
        /// <param name="source">解密前字串</param>
        /// <param name="cryptoKey">解密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        public static byte[] DecryptAES(byte[] source, string cryptoKey, string cryptoIV)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);
            using var aes = System.Security.Cryptography.Aes.Create();
            aes.Mode = System.Security.Cryptography.CipherMode.CBC;
            aes.Padding = System.Security.Cryptography.PaddingMode.None;
            aes.Key = dataKey;
            aes.IV = dataIV;
            using var decryptor = aes.CreateDecryptor();
            byte[] data = decryptor.TransformFinalBlock(source, 0, source.Length);
            int iLength = data[^1];
            var output = new byte[data.Length - iLength];
            Buffer.BlockCopy(data, 0, output, 0, output.Length);
            return output;
        }
    }
}
