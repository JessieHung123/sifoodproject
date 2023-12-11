using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Drivers.Models;
using sifoodproject.Models;

namespace sifoodproject.Areas.Drivers.Controllers
{
    [Area("Drivers")]
    public class HomeController : Controller
    {
        private readonly Sifood3Context _context;

        public HomeController(Sifood3Context context)
        {
            _context = context;
        }
        public IActionResult OrderList()
        {
            return View();
        }
        [Route("/Drivers/Home/ChooseOrder/{OrderId}")]
        public IActionResult ChooseOrder(string OrderId)
        {
            var orderdetail = _context.Orders.Where(o => o.StatusId == 2 && o.OrderId == OrderId);
            return View(orderdetail);
        }

        public IActionResult FinishOrder()
        {
            return View();
        }

        [HttpGet]
        [Route("/Home/FinishOrderData")]
        public IQueryable<OrderDataVM> FinishOrderData()
        {
            string driverId = "D001";

            var orderData = _context.Orders.Include(x => x.User).Include(x => x.Store).Where(y => y.DriverId == driverId && y.StatusId == 5).Select(z => new OrderDataVM
            {
                Address = z.Address,
                OrderDate = z.OrderDate,
                OrderId = z.OrderId,
                ShippingFee = z.ShippingFee,
                StatusName= z.Status.StatusName,
                TotalPrice = z.TotalPrice,
                StoreName = z.Store.StoreName,
                UserName = z.User.UserName
            });
            return orderData;
        }
    }
}
