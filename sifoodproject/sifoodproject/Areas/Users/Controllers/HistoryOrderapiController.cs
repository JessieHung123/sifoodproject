using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Users.Models.ViewModels;
using sifoodproject.Models;
using sifoodproject.Services;

namespace sifoodproject.Areas.Users.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HistoryOrderapiController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserIdentityService _userIdentityService;

        public HistoryOrderapiController(Sifood3Context context, IWebHostEnvironment webHostEnvironment, IUserIdentityService userIdentityService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userIdentityService = userIdentityService;
        }

        // 取得歷史訂單
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<HistoryOrderVM>>> GetHistoryOrders(string searchTerm = "", int pageSize = 5, int pageNumber = 1, string sortOption = "default")
        //{
        //    var query = _context.Orders.AsQueryable();

        //    // 搜索過濾
        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        query = query.Where(o => o.OrderDetails.Any(od => od.Product.ProductName.Contains(searchTerm)));
        //    }

        //    // 排序
        //    switch (sortOption.ToLower())
        //    {
        //        case "priceasc":
        //            query = query.OrderBy(o => o.OrderDetails.Sum(od => od.Quantity * od.Product.UnitPrice) + o.ShippingFee);
        //            break;
        //        case "pricedesc":
        //            query = query.OrderByDescending(o => o.OrderDetails.Sum(od => od.Quantity * od.Product.UnitPrice) + o.ShippingFee);
        //            break;
        //        case "dateasc":
        //            query = query.OrderBy(o => o.OrderDate);
        //            break;
        //        case "datedesc":
        //            query = query.OrderByDescending(o => o.OrderDate);
        //            break;
        //        default:
        //            break; // 保持原有的默認排序方式
        //    }

        //    // 分頁
        //    var totalItems = await query.CountAsync();
        //    var orders = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        //    // 轉換為 ViewModel
        //    var result = orders.Select(o => new HistoryOrderVM
        //    {
        //        StoreId = o.StoreId,
        //        OrderId = o.OrderId,
        //        OrderDate = o.OrderDate,
        //        Status = o.Status.StatusName,
        //        Quantity = o.OrderDetails.Sum(od => od.Quantity),
        //        TotalPrice = Convert.ToInt32(o.OrderDetails.Sum(od => od.Quantity * od.Product.UnitPrice) + o.ShippingFee),
        //        FirstProductPhotoPath = o.OrderDetails.FirstOrDefault()?.Product.PhotoPath,
        //        FirstProductName = o.OrderDetails.FirstOrDefault()?.Product.ProductName,
        //        // 加入 OrderDetails 轉換
        //        OrderDetails = new HistoryOrderDetailVM
        //        {
        //            OrderId = o.OrderId,
        //            OrderDate = o.OrderDate,
        //            // ... 其他相關屬性
        //            Items = o.OrderDetails.Select(od => new HistoryOrderDetailItemVM
        //            {
        //                PhotoPath = od.Product.PhotoPath,
        //                ProductName = od.Product.ProductName,
        //                UnitPrice = od.Product.UnitPrice,
        //                Quantity = od.Quantity
        //            }).ToList()
        //        }
        //    }).ToList();

        //    // 返回分頁信息和數據
        //    return Ok(new { TotalItems = totalItems, Orders = result });
        //}



        //為了改排序先註解掉 然後又復原----------------->
        public async Task<List<HistoryOrderVM>> GetHistoryOrders()
        {
            //var loginUserId = "U002";  // 這裡應該用方法獲取當前用戶ID
            var loginUserId = _userIdentityService.GetUserId();

            var ordersQuery = _context.Orders
                .Where(o => o.UserId == loginUserId)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .Include(o => o.Status);

            var ordersList = await ordersQuery.Select(o => new HistoryOrderVM
            {
                StoreId = o.StoreId,
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                Status = o.Status.StatusName,
                Quantity = o.OrderDetails.Sum(od => od.Quantity),
                TotalPrice = Convert.ToInt32(o.OrderDetails.Sum(od => od.Quantity * od.Product.UnitPrice) + o.ShippingFee),
                FirstProductPhotoPath = o.OrderDetails.FirstOrDefault().Product.PhotoPath,
                FirstProductName = o.OrderDetails.FirstOrDefault().Product.ProductName
            }).ToListAsync();

            return ordersList;
        }
        //------------------>為了改排序先註解掉

        //=========歷史訂單========//
        //public async Task<IEnumerable<HistoryOrderVM>> HistoryOrders(string searchTerm = null, string sortOption = "Status", int pageSize = 20, int currentPage = 1)
        //{

        //    // 當前登入用戶的ID or 寫死ID
        //    //var loginUserId = _userIdentityService.GetUserId();
        //    var loginUserId = "U002";

        //    // 首先應用過濾條件
        //    var historyOrdersQuery = _context.Orders
        //        .Where(o => o.UserId == loginUserId);

        //    if (!string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        historyOrdersQuery = historyOrdersQuery.Where(o =>
        //            o.OrderDetails.Any(od => od.Product.ProductName.Contains(searchTerm)));
        //    }

        //    // 然後使用 Include 來加載關聯實體
        //    historyOrdersQuery = historyOrdersQuery
        //        .Include(o => o.OrderDetails)
        //            .ThenInclude(od => od.Product)
        //        .Include(o => o.Status);

        //    //保持搜尋關鍵字在搜尋欄
        //    //ViewBag.SearchTerm = searchTerm;

        //    //計算總訂單數
        //    var totalOrdersCount = historyOrdersQuery.Count();
        //    //ViewBag.TotalOrdersCount = totalOrdersCount;


        //    // Sort排序
        //    var historyOrdersList = historyOrdersQuery.ToList(); // 將查詢結果轉換為 List

        //    switch (sortOption)
        //    {
        //        case "Status":
        //            historyOrdersList = historyOrdersList.OrderBy(o => o.Status.StatusName).ToList();
        //            break;
        //        case "Low to High":
        //            historyOrdersList = historyOrdersList.OrderBy(o => o.TotalPrice).ToList();
        //            break;
        //        case "High to Low":
        //            historyOrdersList = historyOrdersList.OrderByDescending(o => o.TotalPrice).ToList();
        //            break;
        //        case "Newest":
        //            historyOrdersList = historyOrdersList.OrderByDescending(o => o.OrderDate).ToList();
        //            break;
        //        case "Oldest":
        //            historyOrdersList = historyOrdersList.OrderBy(o => o.OrderDate).ToList();
        //            break;
        //        default:
        //            historyOrdersList = historyOrdersList.OrderByDescending(o => o.OrderDate).ToList();
        //            break;
        //    }


        //    //switch (sortOption)
        //    //{
        //    //    case "Status":
        //    //        historyOrdersQuery = historyOrdersQuery.OrderBy(o => o.Status.StatusName);
        //    //        break;
        //    //    case "Low to High":
        //    //        historyOrdersQuery = historyOrdersQuery.OrderBy(o => o.TotalPrice);
        //    //        break;
        //    //    case "High to Low":
        //    //        historyOrdersQuery = historyOrdersQuery.OrderByDescending(o => o.TotalPrice);
        //    //        break;
        //    //    case "Newest":
        //    //        historyOrdersQuery = historyOrdersQuery.OrderByDescending(o => o.OrderDate);
        //    //        break;
        //    //    case "Oldest":
        //    //        historyOrdersQuery = historyOrdersQuery.OrderBy(o => o.OrderDate);
        //    //        break;
        //    //    default:
        //    //        // 默認排序：按訂購日期由新到舊排序
        //    //        historyOrdersQuery = historyOrdersQuery.OrderByDescending(o => o.OrderDate);
        //    //        break;
        //    //}

        //    // 在過濾後的結果上應用分頁
        //    var paginatedOrders = historyOrdersQuery
        //            .Skip((currentPage - 1) * pageSize)
        //            .Take(pageSize)
        //            .Select(o => new HistoryOrderVM
        //            {
        //                // ViewModel的初始化
        //                StoreId = o.StoreId,
        //                OrderId = o.OrderId,
        //                OrderDate = o.OrderDate,
        //                Status = o.Status.StatusName,
        //                Quantity = o.OrderDetails.Sum(od => od.Quantity),
        //                TotalPrice = Convert.ToInt32(_context.OrderDetails
        //                                            .Where(od => od.OrderId == o.OrderId)
        //                                            .Sum(od => od.Quantity * od.Product.UnitPrice) +
        //                                            o.ShippingFee),
        //                FirstProductPhotoPath = o.OrderDetails.FirstOrDefault().Product.PhotoPath,
        //                FirstProductName = o.OrderDetails.FirstOrDefault().Product.ProductName
        //            }).ToList();

        //    //return Json(new { Orders = paginatedOrders, TotalCount = totalOrdersCount });
        //    return paginatedOrders;
        //}


        [Authorize]
        // 取得訂單明細
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(string orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .Include(o => o.Comment).FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new HistoryOrderDetailVM
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                ShippingFee = order.ShippingFee,
                DeliveryMethod = order.DeliveryMethod,
                Items = order.OrderDetails.Select(od => new HistoryOrderDetailItemVM
                {
                    PhotoPath = od.Product.PhotoPath,
                    ProductName = od.Product.ProductName,
                    UnitPrice = Convert.ToInt32(od.Product.UnitPrice),
                    Quantity = od.Quantity
                }).ToList(),
                CommentRank = order.Comment?.CommentRank,
                CommentContents = order.Comment?.Contents
            };

            return Ok(viewModel);
        }


        //訂單明細方法GetOrderDetails
        //public async Task<IEnumerable<HistoryOrderDetailVM>> GetOrderDetails(string orderId)
        //{
        //    var order = await _context.Orders
        //        .Include(o => o.OrderDetails)
        //            .ThenInclude(od => od.Product)
        //        .Include(o => o.Comment) // 假設 Order 包含多個 Comment
        //        .FirstOrDefaultAsync(o => o.OrderId == orderId);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    var historyOrderDetailsVM = new HistoryOrderDetailVM
        //    {
        //        OrderId = order.OrderId,
        //        OrderDate = order.OrderDate,
        //        ShippingFee = order.ShippingFee,
        //        DeliveryMethod = order.DeliveryMethod,

        //        Items = order.OrderDetails.Select(od => new HistoryOrderDetailItemVM
        //        {
        //            PhotoPath = od.Product.PhotoPath,
        //            ProductName = od.Product.ProductName,
        //            UnitPrice = Convert.ToInt32(od.Product.UnitPrice),
        //            Quantity = od.Quantity
        //        }).ToList(),

        //        CommentRank = order.Comment?.CommentRank,
        //        CommentContents = order.Comment?.Contents
        //    };

        //    return PartialView("_OrderDetailPartial", historyOrderDetailsVM);
        //}


        // 處理評價提交
        [HttpPost]
        public async Task<IActionResult> SubmitRating([FromBody] RatingModel ratingModel)
        {
            if (ratingModel == null || string.IsNullOrEmpty(ratingModel.OrderId))
            {
                return BadRequest("無效的請求數據。");
            }

            var order = await _context.Orders.Include(o => o.Comment).FirstOrDefaultAsync(o => o.OrderId == ratingModel.OrderId);

            if (order == null)
            {
                return NotFound("找不到相關的訂單。");
            }

            if (order.Comment == null)
            {
                order.Comment = new Comment { CommentRank = (short)ratingModel.Rating, Contents = ratingModel.Comment };
            }
            else
            {
                order.Comment.CommentRank = (short)ratingModel.Rating;
                order.Comment.Contents = ratingModel.Comment;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "評價提交成功" });
        }

        //public async Task<IActionResult> SubmitRating([FromBody] RatingModel ratingModel)
        //{
        //    // 檢查從前端接收的評價模型是否為空，或者是否缺少訂單ID
        //    if (ratingModel == null || string.IsNullOrEmpty(ratingModel.OrderId))
        //    {
        //        // 如果數據無效，返回一個錯誤響應
        //        return BadRequest("無效的請求數據。");
        //    }

        //    // 根據訂單ID查找相關的訂單，包括其相關的評論數據
        //    var order = await _context.Orders.Include(o => o.Comment).FirstOrDefaultAsync(o => o.OrderId == ratingModel.OrderId);
        //    if (order == null)
        //    {
        //        // 如果找不到訂單，返回一個未找到的響應
        //        return NotFound("找不到相關的訂單。");
        //    }

        //    // 檢查訂單是否已經有相關聯的評論
        //    if (order.Comment == null)
        //    {
        //        // 如果沒有，則創建一個新的評論實例並賦值
        //        order.Comment = new Comment { CommentRank = (short)ratingModel.Rating, Contents = ratingModel.Comment };
        //    }
        //    else
        //    {
        //        // 如果已經有評論，則更新現有評論的數據
        //        order.Comment.CommentRank = (short)ratingModel.Rating;
        //        order.Comment.Contents = ratingModel.Comment;
        //    }

        //    // 保存更改到數據庫
        //    await _context.SaveChangesAsync();

        //    // 返回操作成功的響應
        //    return Ok(new { message = "評價提交成功" });
        //}

        // 用於從前端接收評價數據的模型類
        public class RatingModel
        {
            public string OrderId { get; set; } // 訂單ID
            public int Rating { get; set; } // 評分數值
            public string Comment { get; set; } // 評論內容
        }


    }
}
