using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Stores.Models;
using sifoodproject.Models;
using sifoodproject.Services;


namespace sifoodproject.Areas.Stores.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductManageapiController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStoreIdentityService _storeIdentityService;

       public ProductManageapiController(Sifood3Context context, IWebHostEnvironment webHostEnvironment, IStoreIdentityService storeIdentityService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _storeIdentityService = storeIdentityService;
        }

        public async Task<List<ProductManageVM>> Filter(string? text)
        {
            string targetStoreId = _storeIdentityService.GetStoreId();

            //找到今天之前上架的商品
            var productsToUpdate = await _context.Products
                .Where(e => e.StoreId == targetStoreId && e.IsDelete == 1 && e.RealeasedTime.Date < DateTime.Now.Date).ToListAsync();

            // 把非今天的產品軟刪除
            foreach (var product in productsToUpdate)
            {
                product.IsDelete = 0;
            }
            await _context.SaveChangesAsync();

            var query = _context.Products.Include(p => p.Category).Where(e => e.StoreId == targetStoreId && e.IsDelete ==1);

            if (!string.IsNullOrEmpty(text))
            {
                query = query.Where(e => e.ProductName.Contains(text) || e.Category.CategoryName.Contains(text));
            }
            return await query.Select(x => new ProductManageVM
            {
                StoreId = x.StoreId,
                UnitPrice = x.UnitPrice,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.CategoryName,
                ReleasedQty = x.ReleasedQty,
                OrderedQty = x.OrderedQty,
                PhotoPath = x.PhotoPath,
                Description = x.Description,
                RealeasedTime = x.RealeasedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                SuggestPickUpTime = x.SuggestPickUpTime,
                SuggestPickEndTime = x.SuggestPickEndTime,
            }).ToListAsync();
        }
        [HttpPut("{id}")]
        public async Task<string> SoftDelete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return "找不到產品！";
            }

            product.IsDelete = 0;
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return "更新成功！";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return "找不到產品！";
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{productId}")]
    public async Task<string> deleteProduct(int productId)
    {
        if (_context.Products == null)
        {
            return "刪除商品失敗!";
        }
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return "刪除商品失敗!";
        }
        try
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

        }
        catch (DbUpdateException)
        {
            return "刪除商品關聯紀錄失敗!";
        }
        return "刪除商品成功!";
    }

    private bool ProductExists(int productId)
        {
            return (_context.Products?.Any(e => e.ProductId == productId)).GetValueOrDefault();
        }


        [HttpPost]
        public async Task<string> postProduct([FromForm]AddProductVM addProductDTO)
        {
            string targetStoreId = _storeIdentityService.GetStoreId();
            //string targetStoreId = "S001";

            Product prodct = new Product
            {
                StoreId = targetStoreId,
                ProductName = addProductDTO.ProductName,
                CategoryId = addProductDTO.CategoryId,
                Description = addProductDTO.Description,
                ReleasedQty = addProductDTO.ReleasedQty,
                UnitPrice = addProductDTO.UnitPrice,
                SuggestPickUpTime=addProductDTO.SuggestPickUpTime,
                SuggestPickEndTime=addProductDTO.SuggestPickEndTime,
                RealeasedTime = DateTime.Now,
                PhotoPath = await SavePhoto(addProductDTO.ImageFile)
            };
            _context.Products.Add(prodct);
            await _context.SaveChangesAsync();
            return "新增成功";
        }

        //儲存照片專用方法
        private async Task<string> SavePhoto(IFormFile photo)
        {
            if (photo != null)
            {
                var fileName = Path.GetFileName(photo.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Products", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                return $"/images/Products/{fileName}";
            }
            return null;
        }

        [HttpPut("{id}")]
        public async Task<string> putProduct(int id,[FromForm] PutProductVM putProductVM)
        {
            string targetStoreId = _storeIdentityService.GetStoreId();

            if (id != putProductVM.ProductId)
            {
                return "修改商品失敗!";
            }
            Product product = await _context.Products.FindAsync(id);
            product.StoreId = targetStoreId;
            product.ProductName = putProductVM.ProductName;
            product.CategoryId = putProductVM.CategoryId;
            product.Description = putProductVM.Description;
            product.ReleasedQty = putProductVM.ReleasedQty;
            product.UnitPrice = putProductVM.UnitPrice;
            product.SuggestPickUpTime = putProductVM.SuggestPickUpTime;
            product.SuggestPickEndTime = putProductVM.SuggestPickEndTime;
           if (putProductVM.ImageFile != null)
            {
                product.PhotoPath = await SavePhoto(putProductVM.ImageFile);
            }

            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return "修改商品失敗!";
                }
                else
                {
                    throw;
                }
            }
            return "修改商品成功!";
        }
    }
}


