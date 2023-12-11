using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Users.Models.ViewModels;
using sifoodproject.Models;
using sifoodproject.Services;

namespace SiFoodProjectFormal2._0.Areas.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressesapiController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IUserIdentityService _userIdentityService;
        int minLength = 10;
        int maxLength = 30;
        public UserAddressesapiController(Sifood3Context context, IWebHostEnvironment webHostEnvironment, IUserIdentityService userIdentityService)
        {
            _context = context;
            _WebHostEnvironment = webHostEnvironment;
            _userIdentityService = userIdentityService;
        }

        // GET: api/UserAddressesapi
        [HttpGet]
        public async Task<List<UserAddressesVM>> GetUserAddress()
        {
            string userId = _userIdentityService.GetUserId();
            return await _context.UserAddresses.Where(u => u.UserId == userId).Select(x => new UserAddressesVM
            {
                UserAddressId = x.UserAddressId,
                UserId = x.UserId,
                UserName = x.User.UserName,
                UserDetailAddress = x.UserDetailAddress,
                UserRegion = x.UserRegion,
                UserCity = x.UserCity,
                UserPhone = x.User.UserPhone,
                IsDefault = x.IsDefault
            }).ToListAsync();
        }

        // PUT: api/UserAddressesapi/5
        [HttpPut("{id}")]
        public async Task<string> PutUserAddress(int id, [FromBody] UserAddressesVM userAddressesVM)
        {
            string userId = _userIdentityService.GetUserId();
            if (string.IsNullOrEmpty(userAddressesVM.UserDetailAddress))
            {
                return "請輸入完整地址";
            }
            if (userAddressesVM.UserDetailAddress.Length < minLength || userAddressesVM.UserDetailAddress.Length > maxLength)
            {
                return $"請輸入長度為{minLength}到{maxLength}字之間的地址";
            }
            if (!IsValidChineseAddress(userAddressesVM.UserDetailAddress))
            {
                return "請輸入正確中文字";
            }

            if (id != userAddressesVM.UserAddressId)
            {
                return "修改地址失敗!";
            }
            UserAddress? userAddresses = await _context.UserAddresses.FindAsync(id);
            if (userAddresses == null)
            {
                return "修改地址失敗!";
            }
            if (userAddressesVM.IsDefault)
            {
                var existingDefaultAddresses = await _context.UserAddresses.Where(a => a.UserId == userId && a.IsDefault && a.UserAddressId != id).ToListAsync();

                foreach (var existingDefaultAddress in existingDefaultAddresses)
                {
                    existingDefaultAddress.IsDefault = false;
                    _context.Entry(existingDefaultAddress).State = EntityState.Modified;
                }
            }
            userAddresses.UserDetailAddress = userAddressesVM.UserDetailAddress;
            userAddresses.IsDefault = userAddressesVM.IsDefault;
            _context.Entry(userAddresses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAddressExists(id))
                {
                    return "修改地址失敗!";
                }
                else
                {
                    throw;
                }
            }

            return "更新成功!";
        }

        // POST: api/UserAddressesapi
        [HttpPost]
        public async Task<string> PostUserAddress([FromForm] UserAddressesVM userAddressesVM)
        {
            string userId = _userIdentityService.GetUserId();
            if (string.IsNullOrEmpty(userAddressesVM.UserRegion) || string.IsNullOrEmpty(userAddressesVM.UserCity) || string.IsNullOrEmpty(userAddressesVM.UserDetailAddress))
            {
                return "請輸入完整地址";
            }
            if (userAddressesVM.UserDetailAddress.Length < minLength || userAddressesVM.UserDetailAddress.Length > maxLength)
            {
                return $"請輸入長度為{minLength}到{maxLength}字之間的地址";
            }
            if (!IsValidChineseAddress(userAddressesVM.UserDetailAddress))
            {
                return "請輸入正確中文字";
            }
            UserAddress userAddresses = new UserAddress
            {
                UserId = userId,
                UserDetailAddress = userAddressesVM.UserDetailAddress,
                UserRegion = userAddressesVM.UserRegion,
                UserCity = userAddressesVM.UserCity,
                IsDefault = userAddressesVM.IsDefault
            };
            if (userAddresses.IsDefault)
            {
                var existingDefaultAddresses = await _context.UserAddresses.Where(a => a.UserId == userAddresses.UserId && a.IsDefault).ToListAsync();
                foreach (var existingDefaultAddress in existingDefaultAddresses)
                {
                    existingDefaultAddress.IsDefault = false;
                    _context.Entry(existingDefaultAddress).State = EntityState.Modified;
                }
            }
            _context.UserAddresses.Add(userAddresses);
            await _context.SaveChangesAsync();
            return "新增地址成功!";
        }

        // DELETE: api/UserAddressesapi/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteUserAddress(int id)
        {
            if (_context.UserAddresses == null)
            {
                return "刪除地址失敗!";
            }
            var userAddress = await _context.UserAddresses.FindAsync(id);
            if (userAddress == null)
            {
                return "刪除地址失敗!";
            }
            try
            {
                _context.UserAddresses.Remove(userAddress);
                await _context.SaveChangesAsync();
            }
           catch (DbUpdateException)
            {
                return "刪除地址關聯紀錄失敗!";
            }

            return "刪除地址成功!";
        }

        private bool UserAddressExists(int id)
        {
            return (_context.UserAddresses?.Any(e => e.UserAddressId == id)).GetValueOrDefault();
        }
        private bool IsValidChineseAddress(string input)
        {
            //中文字及阿拉伯數字
            return Regex.IsMatch(input, @"^[\u4e00-\u9fa50-9]+$");
        }
    }
}
