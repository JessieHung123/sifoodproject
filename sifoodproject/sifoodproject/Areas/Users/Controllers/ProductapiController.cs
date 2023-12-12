using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Users.Models.ViewModels;
using sifoodproject.Models;


namespace sifoodproject.Areas.Users.Controllers
{
    [Route("api/Productapi/[action]")]

    [Area("Users")]
    public class ProductapiController : Controller
    {

        private readonly Sifood3Context _context;

        public ProductapiController(Sifood3Context context)
        {
            _context = context;

        }
        [HttpGet("{id}")]
        public async Task<IEnumerable<ProductVM>> GetProduct(int id)
        {

            var findProduct = _context.Products.Where(p => p.ProductId == id && p.IsDelete == 1 && p.RealeasedTime.Date == DateTime.Now.Date && p.SuggestPickEndTime > DateTime.Now.TimeOfDay).Include(p => p.Store);
            if (findProduct == null) { return Enumerable.Empty<ProductVM>(); }
            else
            {
                return await findProduct.Select(p => new ProductVM
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    PhotoPath = p.PhotoPath,
                    UnitPrice = p.UnitPrice,
                    StoreName = p.Store.StoreName,
                    StoreId = p.StoreId,
                    Address = p.Store.Address,
                    SuggestPickUpTime = p.SuggestPickUpTime.ToString().Substring(0, 5),
                    SuggestPickEndTime = p.SuggestPickEndTime.ToString().Substring(0, 5),
                    ReleasedQty = p.ReleasedQty,
                    OrderedQty = p.OrderedQty,
                    Description=p.Description
                }).ToListAsync();
            }

        }
    }
}
