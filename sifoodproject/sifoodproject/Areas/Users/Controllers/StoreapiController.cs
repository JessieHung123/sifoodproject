using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Users.Models.ViewModels;
using sifoodproject.Models;
using sifoodproject.Services;

namespace SiFoodProjectFormal2._0.Areas.Users.Controllers
{
    [Route("api/Storeapi/[action]")]
    [Area("Users")]
    public class StoreapiController
    {
        private readonly Sifood3Context _context;
        private readonly IUserIdentityService _userIdentityService;
        public StoreapiController(Sifood3Context context, IUserIdentityService userIdentityService)
        {
            _context = context;
            _userIdentityService = userIdentityService;
        }

        public async Task<object> Main2()
        {
            var comment = _context.Comments.AsNoTracking().GroupBy(x => x.StoreId).Select(x => new { x.Key, Count = x.Count() }).ToList();
            var commentRank = _context.Comments.AsNoTracking().GroupBy(x => x.StoreId).Select(x => new { x.Key, TotalRank = x.Sum(z => z.CommentRank) }).ToList();

            var Product = _context.Products.AsNoTracking().Include(x => x.Category).Where(x => x.IsDelete == 1 && x.RealeasedTime.Date == DateTime.Now.Date &&
               x.SuggestPickEndTime >= DateTime.Now.TimeOfDay).GroupBy(x => x.StoreId).Select(x => new { x.Key, sumQty = x.Sum(z => z.ReleasedQty - z.OrderedQty), categoryList = x.Select(z => z.Category.CategoryName) }).ToList();

            var stores = _context.Stores.AsNoTracking().Where(x => x.StoreIsAuthenticated == 1).Select(z => new
            {
                StoreId = z.StoreId,
                StoreName = z.StoreName,
                Description = z.Description,
                LogoPath = z.LogoPath,
                City = z.City,
                Region = z.Region,
                WeekdayOpeningTime = z.OpeningTime.Substring(3, 5),
                WeekdayClosingTime = z.OpeningTime.Substring(11, 5),
                WeekendOpeningTime = z.OpeningTime.Substring(20, 5),
                WeekendClosingTime = z.OpeningTime.Substring(28, 5),
            }).ToList();


            var total = stores.Select(x => new StoreVM
            {
                StoreId = x.StoreId,
                StoreName = x.StoreName,
                Description = x.Description,
                LogoPath = x.LogoPath,
                City = x.City,
                Region = x.Region,
                WeekdayOpeningTime = x.WeekdayOpeningTime,
                WeekdayClosingTime = x.WeekdayClosingTime,
                WeekendOpeningTime = x.WeekendOpeningTime,
                WeekendClosingTime = x.WeekendClosingTime,
                CommentCount = comment.FirstOrDefault(s => s.Key == x.StoreId) == null ? 0 : comment.FirstOrDefault(s => s.Key == x.StoreId).Count,
                CommentRank = commentRank.FirstOrDefault(r => r.Key == x.StoreId)?.TotalRank,
                Inventory = Product.FirstOrDefault(p => p.Key == x.StoreId) == null ? 0 : Product.FirstOrDefault(p => p.Key == x.StoreId).sumQty,
                CategoryName = Product.FirstOrDefault(p => p.Key == x.StoreId) == null ? new List<string>() : Product.FirstOrDefault(p => p.Key == x.StoreId).categoryList
            });

            return total;
        }

        //找到店家是否已被收藏

        [HttpGet]
        public async Task<string[]> GetFavoriteStoreId()
        {

            string userId = _userIdentityService.GetUserId();
            return await _context.Favorites.Where(f => f.UserId == userId).Select(f => f.StoreId).ToArrayAsync();

        }

        [HttpPost]
        public async Task<bool> AddToFavorite([FromBody] Favorite favorite)
        {
            if (favorite == null) return false;

            try
            {
                string userId = _userIdentityService.GetUserId();
                _context.Favorites.Add(new Favorite
                {
                    UserId = userId,
                    StoreId = favorite.StoreId,

                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpDelete]
        public async Task<bool> DeleteFavorite([FromBody] Favorite favorite)
        {

            if (favorite == null) return false;
            try
            {
                string userId = _userIdentityService.GetUserId();
                var likeItem = await _context.Favorites.FirstOrDefaultAsync(c =>
                c.UserId == userId && c.StoreId == favorite.StoreId);
                if (likeItem == null) return false;

                _context.Favorites.Remove(likeItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object FilterInMap()
        {
            var comment = _context.Comments.AsNoTracking().GroupBy(x => x.StoreId).Select(x => new { x.Key, Count = x.Count() }).ToList();
            var commentRank = _context.Comments.AsNoTracking().GroupBy(x => x.StoreId).Select(x => new { x.Key, TotalRank = x.Sum(z => z.CommentRank) }).ToList();

            var Product = _context.Products.AsNoTracking().Include(x => x.Category).Where(x => x.IsDelete == 1 && x.RealeasedTime.Date == DateTime.Now.Date &&
               x.SuggestPickEndTime >= DateTime.Now.TimeOfDay).GroupBy(x => x.StoreId).Select(x => new { x.Key, sumQty = x.Sum(z => z.ReleasedQty - z.OrderedQty), categoryList = x.Select(z => z.Category.CategoryName) }).ToList();

            var stores = _context.Stores.AsNoTracking().Where(x => x.StoreIsAuthenticated == 1).Select(z => new
            {
                StoreId = z.StoreId,
                StoreName = z.StoreName,
                Description = z.Description,
                LogoPath = z.LogoPath,
                PhotosPath = z.PhotosPath,
                PhotosPath2 = z.PhotosPath2,
                PhotosPath3 = z.PhotosPath3,
                City = z.City,
                Region = z.Region,
                WeekdayOpeningTime = z.OpeningTime.Substring(3, 13),
                WeekendOpeningTime = z.OpeningTime.Substring(20, 13),
                Latitude = (decimal)z.Latitude == null ? 0 : (decimal)z.Latitude,
                Longitude = (decimal)z.Longitude == null ? 0 : (decimal)z.Longitude,
                Phone = z.Phone,
                ClosingDay = z.ClosingDay,
                Address = z.Address,
            }).ToList();


            var total = stores.Select(x => new StoreLocationVM
            {
                StoreId = x.StoreId,
                StoreName = x.StoreName,
                Description = x.Description,
                LogoPath = x.LogoPath,
                PhotosPath = x.PhotosPath,
                PhotosPath2 = x.PhotosPath2,
                PhotosPath3 = x.PhotosPath3,
                City = x.City,
                Region = x.Region,
                Address = x.Address,
                ClosingDay = x.ClosingDay,
                WeekdayOpeningTime = x.WeekdayOpeningTime,
                WeekendOpeningTime = x.WeekendOpeningTime,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                CommentCount = comment.FirstOrDefault(s => s.Key == x.StoreId) == null ? 0 : comment.FirstOrDefault(s => s.Key == x.StoreId).Count,
                CommentRank = commentRank.FirstOrDefault(r => r.Key == x.StoreId)?.TotalRank,
                Phone = x.Phone,
                CategoryList = Product.FirstOrDefault(p => p.Key == x.StoreId) == null ? new List<string>() : Product.FirstOrDefault(p => p.Key == x.StoreId).categoryList,
            });

            return total;
        }
    }
}
