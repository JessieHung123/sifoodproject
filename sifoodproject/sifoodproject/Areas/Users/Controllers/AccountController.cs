using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using sifoodproject.Models;
using sifoodproject.Areas.Users.Models.ViewModels;


namespace sifoodproject.Areas.Users.Controllers
{
    [Area("Users")]
    public class AccountController : Controller
    {
        private readonly Sifood3Context _context;
        public AccountController(Sifood3Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult LoginRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginRegister(LoginVM model)
        {
            User? EmailAccount = _context.Users.FirstOrDefault(x => x.UserEmail == model.Account);
            if (EmailAccount != null && EmailAccount.UserAuthenticated == 1)
            {
                string PasswordWithSalt = $"{model.Password}{EmailAccount.UserPasswordSalt}";
                Byte[] RealPasswordBytes = Encoding.ASCII.GetBytes(PasswordWithSalt);
                Byte[] RealPasswordHash = SHA256.HashData(RealPasswordBytes);
                if (Enumerable.SequenceEqual(RealPasswordHash, EmailAccount.UserPasswordHash))
                {
                    EmailAccount.UserLatestLogInDate = DateTime.UtcNow;
                    List<Claim> claims = new()
                        {
                        new(ClaimTypes.Name, $"{EmailAccount.UserId}"),
                        new(ClaimTypes.Role, "User"),
                        };
                    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal, new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddDays(1)
                        });
                    _context.SaveChanges();
                    return RedirectToAction("Main", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        [Route("/Account/PostAccount")]
        public string PostAccount([FromForm] RegisterVM model)
        {
            IQueryable<string> AllAccount = _context.Users.Select(x => x.UserEmail);
            IQueryable<string?> AllUserName = _context.Users.Select(x => x.UserName);
            if (AllAccount.Contains(model.EmailAccount) || AllUserName.Contains(model.UserName))
            {
                return "此帳號已被註冊";
            }
            else
            {
                byte[] saltBytes = new byte[8];
                using (RandomNumberGenerator ran = RandomNumberGenerator.Create())
                {
                    ran.GetBytes(saltBytes);
                }
                byte[] passwordBytes = Encoding.ASCII.GetBytes($"{model?.Password}{saltBytes}");
                byte[] hashBytes = SHA256.HashData(passwordBytes);
                Random UserVerification = new();
                User user = new()
                {
                    UserName = model?.UserName,
                    UserEmail = $"{model?.EmailAccount}",
                    UserPasswordSalt = saltBytes,
                    UserPasswordHash = hashBytes,
                    UserVerificationCode = UserVerification.Next(100000, 999999).ToString(),
                    UserEnrollDate = DateTime.UtcNow,
                    TotalOrderAmount = 0,
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                SmtpClient client = new()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("brad881112@gmail.com", "cttl rkeu vveh ojtv")
                };
                MailMessage mail = new()
                {
                    Subject = "Sifood會員驗證信",
                    From = new MailAddress("brad881112@gmail.com", "Sifood官方帳號")
                };
                mail.To.Add($"{model?.EmailAccount}");
                string MailHtmlBody = $"<div id=\"body\" class=\"sc-jlZhew fuwAaz body\" data-name=\"body\" data-draggable=\"false\" data-empty=\"false\" style=\"background-color: rgb(237, 237, 237);\">\r\n    <div id=\"nsDEeKWBv5AT25uLHham4W\" class=\"sc-jlZhew fuwAaz vertical-frame\" data-name=\"vertical-frame\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: center; padding: 60px 30px 100px; border-radius: 0px; background-color: rgb(26, 33, 32); margin-bottom: unset; --width: 680px;\">\r\n        <div id=\"nC73z7rDaZtb1DnPoZmesl\" class=\"sc-jlZhew fuwAaz vertical-frame\" data-name=\"vertical-frame\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: center; padding: 0px; border-radius: 0px; background-color: unset; --width: 475px;\">\r\n            <div id=\"nWyg-VBoKGfIpL9Xj0qwoq\" src=\"https://uploads.tabular.email/u/88f987f4-4b2f-49a3-bfd1-56a8c4319a80/51fea2b2-7c41-41de-86af-dd2b48532c62.png\" alt=\"\" class=\"sc-jlZhew fuwAaz image\" data-name=\"image\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: flex-start; text-align: left; margin-top: 0px; margin-bottom: 90px; padding: 0px; --width: 40px;\">\r\n                     </div>\r\n            <div id=\"nzk_5G7mxamMm4foEnLJHm\" class=\"sc-jlZhew fuwAaz heading-1\" data-name=\"heading-1\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: center; margin-top: 0px; margin-bottom: 16px; font-family: &quot;Albert Sans&quot;; font-weight: 700; font-style: normal; color: rgb(255, 255, 255); font-size: 35px; letter-spacing: 0px; word-spacing: 0px; line-height: 39px; text-align: left; text-transform: none; text-decoration: none; direction: ltr; --width: 475px; padding-bottom: 0px;\">\r\n                <span id=\"nCayFSnnkGGRlSTMgTwG7k\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\">帳號驗證</span>\r\n            </div>\r\n            <div id=\"n8eO6sS8lPQIrh69cr9M49\" class=\"sc-jlZhew fuwAaz paragraph\" data-name=\"paragraph\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: flex-start; margin-top: 0px; margin-bottom: 56px; font-family: &quot;Albert Sans&quot;; font-weight: 400; font-style: normal; color: rgb(255, 255, 255); font-size: 21px; letter-spacing: 0px; word-spacing: 0px; line-height: 32px; text-align: left; text-transform: none; text-decoration: none; direction: ltr; --width: 430px;\">\r\n                <span id=\"nkqntWTTVhUjDKRCzOLEKf\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\">為了驗證您的帳號, 請幫我在驗證畫面輸入以下驗證碼</span>\r\n            </div>\r\n            <div id=\"nCyOjg661zdZ7IUK7vV47T\" href=\"https://tabular.email\" class=\"sc-jlZhew fuwAaz button\" data-name=\"button\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: flex-start; font-family: &quot;Inter Tight&quot;; font-weight: 800; font-style: normal; text-align: center; text-decoration: none; font-size: 14px; line-height: 48px; color: rgb(255, 255, 255); background-color: rgb(17, 152, 114); margin-top: 0px; margin-bottom: 0px; padding: 0px; letter-spacing: 0.65px; word-spacing: 0px; direction: ltr; border-radius: 4px; text-transform: uppercase; --width: 246px;\">\r\n                <span id=\"nONbX9vLZTyWRQAlCXgYjO\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\">您的驗證碼為 : {user.UserVerificationCode}</span>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>";
                mail.Body = MailHtmlBody;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                client.Send(mail);
                return "帳號註冊成功, 即將進入驗證階段";
            }
        }

        [HttpPost]
        [Route("/Account/OpenUserAccount")]
        public string OpenUserAccount([FromBody] EmailVerificationVM model)
        {
            User? LockUser = _context.Users.FirstOrDefault(x => x.UserVerificationCode == model.UserAccountVerificationCode);
            if (LockUser != null)
            {
                LockUser.UserAuthenticated = 1;
                _context.SaveChanges();
                return "驗證帳號成功，已為您開通帳號，請再次重新登入";
            }
            else
            {
                return "驗證帳號失敗，請重新輸入驗證碼";
            }
        }

        [HttpPost]
        [Route("/Account/ForgotPasswordSendEmail")]
        public string ForgotPasswordSendEmail([FromBody] LoginVM model)
        {
            User? user = _context.Users.FirstOrDefault(x => x.UserEmail == model.Account);
            if (user == null)
            {
                return "找不到此使用者";
            }
            else
            {
                Random UserVerification = new();
                user.ForgotPasswordRandom = UserVerification.Next(100000, 999999).ToString();
                _context.SaveChanges();
                SmtpClient client = new()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("brad881112@gmail.com", "cttl rkeu vveh ojtv")
                };
                MailMessage mail = new()
                {
                    Subject = "Sifood會員驗證信",
                    From = new MailAddress("brad881112@gmail.com", "Sifood官方帳號")
                };
                mail.To.Add($"{model.Account}");
                string MailHtmlBody = $"<div id=\"body\" class=\"sc-jlZhew fuwAaz body\" data-name=\"body\" data-draggable=\"false\" data-empty=\"false\" style=\"background-color: rgb(237, 237, 237);\">\r\n    <div id=\"nsDEeKWBv5AT25uLHham4W\" class=\"sc-jlZhew fuwAaz vertical-frame\" data-name=\"vertical-frame\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: center; padding: 60px 30px 100px; border-radius: 0px; background-color: rgb(26, 33, 32); margin-bottom: unset; --width: 680px;\">\r\n        <div id=\"nC73z7rDaZtb1DnPoZmesl\" class=\"sc-jlZhew fuwAaz vertical-frame\" data-name=\"vertical-frame\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: center; padding: 0px; border-radius: 0px; background-color: unset; --width: 475px;\">\r\n            <div id=\"nWyg-VBoKGfIpL9Xj0qwoq\" src=\"https://uploads.tabular.email/u/88f987f4-4b2f-49a3-bfd1-56a8c4319a80/51fea2b2-7c41-41de-86af-dd2b48532c62.png\" alt=\"\" class=\"sc-jlZhew fuwAaz image\" data-name=\"image\" data-draggable=\"true\" data-empty=\"false\" style=\"align-self: flex-start; text-align: left; margin-top: 0px; margin-bottom: 90px; padding: 0px; --width: 40px;\">\r\n                     </div>\r\n            <div id=\"nzk_5G7mxamMm4foEnLJHm\" class=\"sc-jlZhew fuwAaz heading-1\" data-name=\"heading-1\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: center; margin-top: 0px; margin-bottom: 16px; font-family: &quot;Albert Sans&quot;; font-weight: 700; font-style: normal; color: rgb(255, 255, 255); font-size: 35px; letter-spacing: 0px; word-spacing: 0px; line-height: 39px; text-align: left; text-transform: none; text-decoration: none; direction: ltr; --width: 475px; padding-bottom: 0px;\">\r\n                <span id=\"nCayFSnnkGGRlSTMgTwG7k\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\">帳號驗證</span>\r\n            </div>\r\n            <div id=\"n8eO6sS8lPQIrh69cr9M49\" class=\"sc-jlZhew fuwAaz paragraph\" data-name=\"paragraph\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: flex-start; margin-top: 0px; margin-bottom: 56px; font-family: &quot;Albert Sans&quot;; font-weight: 400; font-style: normal; color: rgb(255, 255, 255); font-size: 21px; letter-spacing: 0px; word-spacing: 0px; line-height: 32px; text-align: left; text-transform: none; text-decoration: none; direction: ltr; --width: 430px;\">\r\n                <span id=\"nkqntWTTVhUjDKRCzOLEKf\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\">為了驗證您的帳號, 請幫我在驗證畫面輸入以下驗證碼</span>\r\n            </div>\r\n            <div id=\"nCyOjg661zdZ7IUK7vV47T\" href=\"https://tabular.email\" class=\"sc-jlZhew fuwAaz button\" data-name=\"button\" data-draggable=\"true\" data-empty=\"false\" contenteditable=\"false\" spellcheck=\"false\" autocomplete=\"off\" autocorrect=\"off\" aria-multiline=\"true\" role=\"textbox\" style=\"align-self: flex-start; font-family: &quot;Inter Tight&quot;; font-weight: 800; font-style: normal; text-align: center; text-decoration: none; font-size: 14px; line-height: 48px; color: rgb(255, 255, 255); background-color: rgb(17, 152, 114); margin-top: 0px; margin-bottom: 0px; padding: 0px; letter-spacing: 0.65px; word-spacing: 0px; direction: ltr; border-radius: 4px; text-transform: uppercase; --width: 246px;\">\r\n                <span id=\"nONbX9vLZTyWRQAlCXgYjO\" data-type=\"TEXT_NODE\" data-name=\"#text\" style=\"\">您的驗證碼為 : {user.ForgotPasswordRandom}</span>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>";
                mail.Body = MailHtmlBody;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                client.Send(mail);
                return "驗證碼寄送成功";
            }
        }

        [HttpPost]
        [Route("/Account/ConfirmForgotPasswordRandom")]
        public string ConfirmForgotPasswordRandom([FromBody] EmailVerificationVM model)
        {
            User? user = _context.Users.FirstOrDefault(x => x.ForgotPasswordRandom == model.UserAccountVerificationCode);
            if (user == null)
            {
                return "驗證碼核對失敗";
            }
            else
            {
                return "驗證碼核對成功";
            }
        }

        [HttpPost]
        [Route("/Account/ResetPassword")]
        public string ResetPassword([FromForm] ResetPasswordVM model)
        {
            User? user = _context.Users.FirstOrDefault(x => x.UserEmail == model.UserConfirmEmail);
            if (user != null)
            {
                byte[] NewSaltBytes = new byte[8];
                using (RandomNumberGenerator ran = RandomNumberGenerator.Create())
                {
                    ran.GetBytes(NewSaltBytes);
                }
                user.UserPasswordSalt = NewSaltBytes;
                byte[] PasswordBytes = Encoding.ASCII.GetBytes($"{model?.NewPassword}{NewSaltBytes}");
                byte[] NewHashBytes = SHA256.HashData(PasswordBytes);
                user.UserPasswordHash = NewHashBytes;
                _context.SaveChanges();
                return "密碼重設成功, 即將回到登入畫面";
            }
            else
            {
                return "使用者核對失敗";
            }
        }

        [HttpPost]
        [Route("/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Main", "Home");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult RegisterConfirmation()
        {
            return View();

        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}