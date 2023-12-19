﻿using Microsoft.AspNetCore.Authorization;
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

        public async Task<List<HistoryOrderVM>> GetHistoryOrders()
        {
            //var loginUserId = "U002";  // 這裡應該用方法獲取當前用戶ID
            var loginUserId = _userIdentityService.GetUserId();

            var ordersQuery = _context.Orders
                .Where(o => o.UserId == loginUserId)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .Include(o => o.Status).Where(s=>s.Status.StatusId==5|| s.Status.StatusId ==6|| s.Status.StatusId ==7);

            var ordersList = await ordersQuery.Select(o => new HistoryOrderVM
            {
                StoreId = o.StoreId,
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                Status = o.Status.StatusName,
                Quantity = o.OrderDetails.Sum(od => od.Quantity),
                TotalPrice = Convert.ToInt32(o.TotalPrice),
                //TotalPrice = Convert.ToInt32(o.OrderDetails.Sum(od => od.Quantity * od.Product.UnitPrice) + o.ShippingFee),
                FirstProductPhotoPath = o.OrderDetails.FirstOrDefault().Product.PhotoPath,
                FirstProductName = o.OrderDetails.FirstOrDefault().Product.ProductName
            }).ToListAsync();

            return ordersList;
        }
       
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

        [HttpPost]
        public async Task<object> SubmitRating([FromBody] RatingVM ratingModel)
        {
            if (ratingModel == null || ratingModel.OrderId == "")
            {
                return BadRequest("無效的請求數據。");
            }

            //var order = await _context.Orders.Include(o => o.Comment).FirstOrDefaultAsync(o => o.OrderId == ratingModel.OrderId);
            var order = await _context.Orders
                            .Include(o => o.Comment)
                            .Include(o => o.Status) 
                            .FirstOrDefaultAsync(o => o.OrderId == ratingModel.OrderId);

            if (order == null)
            {
                return NotFound("找不到相關的訂單。");
            }

            // 檢查訂單的 StatusID 是否為 7
            if (order.StatusId == 7)
            {
                return BadRequest("已取消");
            }


            if (order.Comment == null)
            {
                var newComment = new Comment
                {
                    CommentTime = DateTime.Now,
                    CommentRank = (short)ratingModel.Rating,
                    Contents = ratingModel.Comment,
                    OrderId = ratingModel.OrderId,
                    StoreId = _context.Orders.Where(x => x.OrderId == ratingModel.OrderId).Select(x => x.StoreId).FirstOrDefault()
                };
                _context.Comments.Add(newComment);
                await _context.SaveChangesAsync();
            }
            else
            {
                order.Comment.CommentRank = (short)ratingModel.Rating;
                order.Comment.Contents = ratingModel.Comment;
            }
            return Ok(new { message = "評價提交成功" });
        }
        
        //移動去當ViewModel
        //public class RatingVM
        //{
        //    public string OrderId { get; set; } // 訂單ID
        //    public int Rating { get; set; } // 評分數值
        //    public string Comment { get; set; } // 評論內容 
        //}
    }
}
