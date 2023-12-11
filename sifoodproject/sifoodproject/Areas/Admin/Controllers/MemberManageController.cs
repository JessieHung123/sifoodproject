using Microsoft.AspNetCore.Mvc;
using sifoodproject.Models;


namespace sifoodproject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberManageController : Controller
    {
        private readonly Sifood3Context _context;

        public MemberManageController(Sifood3Context context)
        {
            _context = context;
        }
        //分頁+查詢
        public IActionResult Index(int page = 1, int pageSize = 5, string searchUsers = null)
        {
            if (!string.IsNullOrEmpty(searchUsers))
            {
                TempData["searchUsers"] = searchUsers;
            }
            else
            {
                searchUsers = TempData["searchUsers"] as string ?? "";
            }

            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchUsers))
            {
                query = query.Where(u => u.UserName.Contains(searchUsers) || u.UserEmail.Contains(searchUsers));
            }

            var totalEntries = query.Count();
            var users = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var entriesStart = (page - 1) * pageSize + 1;
            var entriesEnd = entriesStart + users.Count - 1;

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalEntries / (double)pageSize);
            ViewBag.EntriesStart = entriesStart;
            ViewBag.EntriesEnd = entriesEnd;
            ViewBag.TotalEntries = totalEntries;

            return View(users);
        }

        public IActionResult Details1(string UserId)
        {
            User user = _context.Users.Where(x => x.UserId == UserId).FirstOrDefault();
            return PartialView("_Details", user);
        }

        public IActionResult Edit1(string UserId)
        {
            User user = _context.Users.Where(x => x.UserId == UserId).FirstOrDefault();
            return PartialView("_Edit", user);
        }

        [HttpPost]
        public IActionResult Edit1(User user)
        {           
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete1(string UserId)
        {
            User user = _context.Users.Where(x => x.UserId == UserId).FirstOrDefault();
            return PartialView("_Delete", user);
        }

        [HttpPost]
        public IActionResult Delete1(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
