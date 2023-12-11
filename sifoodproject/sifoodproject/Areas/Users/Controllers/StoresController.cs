using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Models;

namespace SiFoodProjectFormal2._0.Areas.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StoresController(Sifood3Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Stores
        [HttpGet]
        public async Task<IEnumerable<Store>> GetStores()
        {
          
            return  _context.Stores;
        }

        // GET: api/Stores/5
        [HttpGet("{id}")]
        //public object GetStore(string id)

        //{
        //    var today = DateTime.Today;
        //    var currentTime = DateTime.Now.TimeOfDay;
        //    var store = _context.Stores.AsNoTracking().Include(x => x.Products).Include(x => x.Orders)
        //       .ThenInclude(x => x.Comment).Where(c => c.StoreId == id);
            
        //    return store.Select(z => new StoreProductsVM
        //    {
        //           StoreName = z.StoreName,
        //           StoreId = z.StoreId,                  
        //           Email = z.Email,
        //           Phone = $"{z.Phone.Substring(0, 2)} {z.Phone.Substring(2)}",
        //           Address = z.Address,
        //           OpeningTime = z.OpeningTime,
        //           PhotosPath = z.PhotosPath,
        //           Description = z.Description,
        //           LogoPath = z.LogoPath,
        //           CommentCount = z.Orders.Where(x => x.Comment != null).Count(),
        //           CommentRank = z.Orders.Sum(x => x.Comment.CommentRank),
        //           WeekdayOpeningTime = z.OpeningTime.Substring(0, 16),
        //           WeekendOpeningTime = z.OpeningTime.Substring(17, 16),

        //           Products = z.Products.Where(p=>p.RealeasedTime.Date==today&&p.RealeasedTime.TimeOfDay<currentTime&&p.SuggestPickEndTime> currentTime).Select(p => new ProductsVM
        //           {
        //               UnitPrice = p.UnitPrice,
        //               ProductName = p.ProductName,
        //               CategoryId = p.CategoryId,
        //               CategoryName = p.Category.CategoryName,
        //               avalibleQty = p.ReleasedQty - p.OrderedQty,
        //               SuggestPickUpTime = $"{ p.SuggestPickUpTime.ToString(@"hh\:mm") } ~ { p.SuggestPickEndTime.ToString(@"hh\:mm")}",
        //               RealeasedTime = p.RealeasedTime,
        //               PhotoPath = p.PhotoPath,
        //           }),

        //           CategoryList =   z.Products.Where(p => p.RealeasedTime.Date == today && p.RealeasedTime.TimeOfDay < currentTime && p.SuggestPickEndTime > currentTime).Select(y => y.Category.CategoryName).Distinct().ToArray(),

        //           Comment = z.Orders.Select(d => new CommentVM
        //           {
        //               Contents = d.Comment.Contents,
        //               CommentRank = d.Comment.CommentRank,
        //               User = d.User.UserName,
        //               DeliveryMethod = d.DeliveryMethod
        //           })
        //       });
        //}

        // PUT: api/Stores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Stores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Store>> PostStore(Store store)
        {
          if (_context.Stores == null)
          {
              return Problem("Entity set 'Sifood3Context.Stores'  is null.");
          }
            _context.Stores.Add(store);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoreExists(store.StoreId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStore", new { id = store.StoreId }, store);
        }

        // DELETE: api/Stores/5
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
