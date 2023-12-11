using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Stores.Models;
using sifoodproject.Models;
using sifoodproject.Services;


namespace sifoodproject.Areas.Stores.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InfoModifyapiController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IStoreIdentityService _storeIdentityService;


        public InfoModifyapiController(Sifood3Context context, IStoreIdentityService storeIdentityService)
        {
            _context = context;
            _storeIdentityService = storeIdentityService;
        }

        public async Task<List<InfoModifyVM>> GetAll()
        {
            string targetStoreId = _storeIdentityService.GetStoreId();
            return await _context.Stores
                .Where(e => e.StoreId == targetStoreId).Select(x => new InfoModifyVM
                {
                    StoreId = x.StoreId,
                    StoreName = x.StoreName,
                    ContactName = x.ContactName,
                    Email = x.Email,
                    Phone = x.Phone,
                    City = x.City,
                    Region = x.Region,
                    Address = x.Address,
                    Description = x.Description,
                    OpeningTime = x.OpeningTime,
                    ClosingDay = x.ClosingDay,
                    PhotosPath = x.PhotosPath,
                    LogoPath = x.LogoPath,
                    PhotosPath2 = x.PhotosPath2,
                    PhotosPath3 = x.PhotosPath3,
                    WeekdayOpen= x.OpeningTime.Substring(3, 5),
                    WeekdayClose= x.OpeningTime.Substring(11, 5),
                    WeekendOpen= x.OpeningTime.Substring(20, 5),
                    WeekendClose= x.OpeningTime.Substring(28, 5)
                }).ToListAsync();
        }

        //存到店家logo的路徑
        private async Task<string> SaveLogo(IFormFile photo)
        {
            if (photo != null)
            {
                var fileName = Path.GetFileName(photo.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Stores/logo", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                return $"/images/Stores/logo/{fileName}";
            }
            return null;
        }
        //存到店家照片的路徑
        private async Task<string> SavePhoto(IFormFile photo)
        {
            if (photo != null)
            {
                var fileName = Path.GetFileName(photo.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Stores/photo", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                return $"/images/Stores/photo/{fileName}";
            }
            return null;
        }

        [HttpPut("{id}")]
        public async Task<string> PutInfoModify(string id, [FromForm] PutInfoModifyVM putInfoModifyVM)
        {

            if (id != putInfoModifyVM.StoreId)
            {
                return "修改商品失敗!";
            }
            Store store = await _context.Stores.FindAsync(id);
            store.StoreName = putInfoModifyVM.StoreName;
            store.ContactName = putInfoModifyVM.ContactName;
            store.Email = putInfoModifyVM.Email;
            store.Phone = putInfoModifyVM.Phone;
            store.City = putInfoModifyVM.City;
            store.Region = putInfoModifyVM.Region;
            store.Address = putInfoModifyVM.Address;
            store.Description = putInfoModifyVM.Description;
            store.OpeningTime = putInfoModifyVM.OpeningTime;
            store.ClosingDay = putInfoModifyVM.ClosingDay;
            if (putInfoModifyVM.LogoImageFile != null)
            {
                store.LogoPath = await SaveLogo(putInfoModifyVM.LogoImageFile);
            }
            if (putInfoModifyVM.PhotoImageFile1 != null)
            {
                store.PhotosPath = await SavePhoto(putInfoModifyVM.PhotoImageFile1);
            }
            if (putInfoModifyVM.PhotoImageFile2 != null)
            {
                store.PhotosPath2 = await SavePhoto(putInfoModifyVM.PhotoImageFile2);
            }
            if (putInfoModifyVM.PhotoImageFile3 != null)
            {
                store.PhotosPath3 = await SavePhoto(putInfoModifyVM.PhotoImageFile3);
            }

            _context.Entry(store).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return "修改店家資料失敗!";
                }
                else
                {
                    throw;
                }
            }
            return "修改店家資料成功!";


        }

        private bool ProductExists(string storeId)
        {
            return (_context.Stores?.Any(e => e.StoreId == storeId)).GetValueOrDefault();
        }




    }

}
