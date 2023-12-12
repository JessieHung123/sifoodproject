using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Stores.Models;
using sifoodproject.Models;
using sifoodproject.Services;

namespace sifoodproject.Areas.Stores.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewapiController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IStoreIdentityService _storeIdentityService;

        public ReviewapiController(Sifood3Context context, IStoreIdentityService storeIdentityService)
        {
            _context = context;
            _storeIdentityService = storeIdentityService;

        }

        public async Task<List<ReviewVM>> Filter(string? text)
        {
            string targetStoreId = _storeIdentityService.GetStoreId();
            //string targetStoreId = "S001";

            var review = _context.Orders.Include(p => p.Comment)
                .Include(p => p.User)
                .Where(x => x.Comment != null)
                .Where(e => e.StoreId == targetStoreId);

            if (!string.IsNullOrEmpty(text))
            {
                review = review.Where(e => e.User.UserName.Contains(text) || e.Comment.Contents.Contains(text));
            }
            return await review.Select(x => new ReviewVM
                {
                    UserId = x.UserId,
                    OrderId = x.OrderId,
                    CommentRank = x.Comment.CommentRank,
                    CommentTime = x.Comment.CommentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Contents = x.Comment.Contents,
                    StoreId = x.StoreId,
                    UserName = x.User.UserName
                }).ToListAsync();
        }

        public async Task<List<ReviewVM>> SelectStars(int? num)
        {
            string targetStoreId = _storeIdentityService.GetStoreId();
            //string targetStoreId = "S001";

            var review = _context.Orders.Include(p => p.Comment)
                .Include(p => p.User)
                .Where(x => x.Comment != null)
                .Where(e => e.StoreId == targetStoreId);
            if (num != 0)
            {
                review = review.Where(e => e.Comment.CommentRank == num);
            }
            return await review.Select(x => new ReviewVM
            {
                UserId = x.UserId,
                OrderId = x.OrderId,
                CommentRank = x.Comment.CommentRank,
                CommentTime = x.Comment.CommentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Contents = x.Comment.Contents,
                StoreId = x.StoreId,
                UserName = x.User.UserName
            }).ToListAsync();
        }

    }
}
