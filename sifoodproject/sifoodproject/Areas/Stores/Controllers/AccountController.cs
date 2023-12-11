using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using sifoodproject.Models;
using sifoodproject.Areas.Stores.Models;

namespace sifoodproject.Areas.Stores.Controllers
{
    [Area("Stores")]
    public class AccountController : Controller
    {
        private readonly Sifood3Context _context;

        public AccountController(Sifood3Context context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SetAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(StoreLoginVM model)
        {
            Store? account = _context.Stores.FirstOrDefault(x => x.Email == model.StoreAccount);
            if (account != null)
            {
                string passwordWithSalt = $"{model.SetPassword}{account.PasswordSalt}";
                Byte[] RealPasswordBytes = Encoding.ASCII.GetBytes(passwordWithSalt);
                Byte[] RealPasswordHash = SHA256.HashData(RealPasswordBytes);
                if (Enumerable.SequenceEqual(RealPasswordHash, account.PasswordHash))
                {
                    List<Claim> claims = new()
                        {
                        new Claim(ClaimTypes.Name, $"{account.StoreId}"),
                        new Claim(ClaimTypes.Role, "Store"),
                        };
                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal, new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddDays(1)
                        });
                    return RedirectToAction("Main", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        [Route("Account/SetPassword")]
        public string SetPassword([FromForm] StoreLoginVM model)
        {
            Store? account = _context.Stores.FirstOrDefault(x => x.Email == model.StoreAccount);
            if (account == null)
            {
                return "找不到此商家";
            }
            else
            {
                byte[] saltBytes = new byte[8];
                using (RandomNumberGenerator ran = RandomNumberGenerator.Create())
                {
                    ran.GetBytes(saltBytes);
                }
                byte[] passwordBytes = Encoding.ASCII.GetBytes($"{model?.SetPassword}{saltBytes}");
                byte[] hashBytes = SHA256.HashData(passwordBytes);
                account.PasswordSalt = saltBytes;
                account.PasswordHash = hashBytes;
                account.StoreIsAuthenticated = 1;
                _context.SaveChanges();
                return "密碼設定成功，請重新登入";
            }
        }

        [HttpGet]
        [Route("/Account/StoreLogout")]
        public async Task<IActionResult> StoreLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login","Account");
        }
    }
}
