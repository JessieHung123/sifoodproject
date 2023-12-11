using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Users.Models.ViewModels;
using sifoodproject.Models;
using sifoodproject.Services;


namespace SiFoodProjectFormal2._0.Areas.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreProductsapiController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserIdentityService _userIdentityService;
        public StoreProductsapiController(Sifood3Context context, IWebHostEnvironment webHostEnvironment, IUserIdentityService userIdentityService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userIdentityService = userIdentityService;
        }

        // GET: api/StoreProductsapi
        [HttpGet]
        public async Task<IEnumerable<Store>> GetStores()
        {
            return _context.Stores;
        }

        // GET: api/StoreProductsapi/5
        [HttpGet("{id}")]
        public async Task<List<StoreProductsVM>> GetStore(string id)
        {
            var today = DateTime.Today;
            var currentTime = DateTime.Now.TimeOfDay;
            return await _context.Stores.AsNoTracking()
                .Include(x => x.Products)
                .Include(x => x.Orders)
                .ThenInclude(x => x.Comment)
                .Where(c => c.StoreId == id)
                .Select(z => new StoreProductsVM
                {
                    StoreName = z.StoreName,
                    StoreId = z.StoreId,
                    Email = z.Email,
                    Phone = $"{z.Phone.Substring(0, 2)} {z.Phone.Substring(2, 4)} {z.Phone.Substring(4, 4)}",
                    Address = z.Address,
                    OpeningTime = z.OpeningTime,
                    PhotosPath = z.PhotosPath,
                    Description = z.Description,
                    LogoPath = z.LogoPath,
                    CommentCount = z.Orders.Where(x => x.Comment != null).Count(),
                    CommentRank = z.Orders.Sum(x => x.Comment.CommentRank),
                    WeekdayOpeningTime = z.OpeningTime.Substring(0, 16),
                    WeekendOpeningTime = z.OpeningTime.Substring(17, 16),
                    OpenForBusiness = CheckOpenTime(z.OpeningTime, currentTime),
                    Products = z.Products.Where(p => p.RealeasedTime.Date == today &&
                                                     p.RealeasedTime.TimeOfDay < currentTime &&
                                                     p.SuggestPickEndTime > currentTime &&
                                                     p.IsDelete == 1)
                                     .Select(p => new ProductsVM
                                     {
                                         ProductId = p.ProductId,
                                         UnitPrice = p.UnitPrice,
                                         ProductName = p.ProductName,
                                         CategoryId = p.CategoryId,
                                         CategoryName = p.Category.CategoryName,
                                         avalibleQty = p.ReleasedQty - p.OrderedQty,
                                         SuggestPickUpTime = $"{p.SuggestPickUpTime.ToString(@"hh\:mm")} ~ {p.SuggestPickEndTime.ToString(@"hh\:mm")}",
                                         RealeasedTime = p.RealeasedTime,
                                         PhotoPath = p.PhotoPath,
                                     }),

                    CategoryList = z.Products.Where(p => p.RealeasedTime.Date == today &&
                                                         p.RealeasedTime.TimeOfDay < currentTime &&
                                                         p.SuggestPickEndTime > currentTime &&
                                                         p.IsDelete == 1)
                                         .Select(y => y.Category.CategoryName).Distinct().ToArray(),

                    Comment = z.Orders
                    .Where(x => x.Comment != null)
                    .Select(d => new CommentVM
                    {
                        Contents = d.Comment.Contents,
                        CommentRank = d.Comment.CommentRank,
                        User = d.User.UserName,
                        DeliveryMethod = d.DeliveryMethod
                    })
                }).ToListAsync();
        }
        [HttpGet("favorite/status/{storeId}")]
        public async Task<object> GetFavoriteStatus(string storeId)
        {
            string userId = _userIdentityService.GetUserId();
            //return _context.Favorites.Any(f => f.UserId == userId && f.StoreId == storeId);
            bool isFavorite = await _context.Favorites.AnyAsync(f => f.UserId == userId && f.StoreId == storeId);
            return Ok(new { IsFavorite = isFavorite });
        }
        [HttpPost("favorite/add")]
        public async Task<string> SaveToFavorites([FromBody] StoreFavoriteVM favoriteVM)
        {
            //string userId = favoriteVM.UserId;
            string userId = _userIdentityService.GetUserId();
            string storeId = favoriteVM.StoreId;
            if (favoriteVM == null || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(storeId))
            {
                return "無法新增";
            }
            if (!_context.Favorites.Any(f => f.UserId == userId && f.StoreId == storeId))
            {
                Favorite favorite = new Favorite
                {
                    UserId = userId,
                    StoreId = favoriteVM.StoreId
                };
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
                return "收藏成功!";
            }
            return "收藏成功!";
        }
        [HttpDelete("favorite/remove")]
        public async Task<string> RemoveFromFavorites([FromBody] StoreFavoriteVM favoriteVM)
        {
            string userId = _userIdentityService.GetUserId();
            if (favoriteVM == null || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(favoriteVM.StoreId))
            {
                return "未收藏";
            }

            var existingFavorite = _context.Favorites
                .FirstOrDefault(f => f.UserId == userId && f.StoreId == favoriteVM.StoreId);

            if (existingFavorite != null)
            {
                _context.Favorites.Remove(existingFavorite);
                await _context.SaveChangesAsync();
                return "已取消收藏";
            }

            return "未收藏";

        }
        private static bool CheckOpenTime(string openingTime, TimeSpan currentTime)
        {
            var weekdaysOpeningTime = openingTime.Substring(3, 13);
            var weekendsOpeningTime = openingTime.Substring(20, 13);

            var applicableOpeningTime = DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? weekendsOpeningTime : weekdaysOpeningTime;

            var parsedOpeningTime = TimeSpan.Parse(applicableOpeningTime.Substring(0, 5));
            var parsedClosingTime = TimeSpan.Parse(applicableOpeningTime.Substring(8, 5));

            return parsedOpeningTime <= currentTime && currentTime <= parsedClosingTime;
        }
     
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(string id, Store store)
        {
            if (id != store.StoreId)
            {
                return BadRequest();
            }

            _context.Entry(store).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StoreProductsapi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Store>> PostStore(Store store)
        {
            if (_context.Stores == null)
            {
                return Problem("Entity set 'Sifood3Context.Stores'  is null.");
            }
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStore", new { id = store.StoreId }, store);
        }

        // DELETE: api/StoreProductsapi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(string id)
        {
            if (_context.Stores == null)
            {
                return NotFound();
            }
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoreExists(string id)
        {
            return (_context.Stores?.Any(e => e.StoreId == id)).GetValueOrDefault();
        }
    }
}
