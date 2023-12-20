using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using sifoodproject.Areas.Admin.Models;
using sifoodproject.Areas.Admin.NewFolder;
using sifoodproject.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace sifoodproject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly Sifood3Context _context;
        public HomeController(Sifood3Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var admin = _context.Admins.Where(x => x.Account == model.Account).FirstOrDefault();
            if (admin != null)
            {
                string passwordWithSalt = $"{model.Password}{admin.PasswordSalt}";
                Byte[] RealPasswordBytes = Encoding.ASCII.GetBytes(passwordWithSalt);
                Byte[] RealPasswordHash = SHA256.HashData(RealPasswordBytes);
                if (Enumerable.SequenceEqual(RealPasswordHash, admin.Password))
                {
                    List<Claim> claims = new()
                        {
                        new Claim(ClaimTypes.NameIdentifier, $"{admin.Account}"),
                        new Claim(ClaimTypes.Role, "Admin"),
                        };
                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new(identity);
                   await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });
                    return RedirectToAction("Index", "OrderManage");
                }
            }
            return View();
        }
        public IActionResult AdminChatRoom()
        {
            return View();
        }
    }
}
