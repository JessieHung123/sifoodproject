using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Stores.Models;
using sifoodproject.Models;
using sifoodproject.Services;


namespace sifoodproject.Areas.Stores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreRealTimeOrdersapiController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IStoreIdentityService _storeIdentityService;
        public StoreRealTimeOrdersapiController(Sifood3Context context, IWebHostEnvironment webHostEnvironment, IStoreIdentityService storeIdentityService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _storeIdentityService = storeIdentityService;
        }

        //[HttpGet]
        //public void ScheduleOrderStatusCheck()
        //{
        //    RecurringJob.AddOrUpdate("CheckUnconfirmedOrdersJob", () => CheckUnconfirmedOrders(), "*/1 * * * *");
        //}


        [HttpGet("filter")]
        public async Task<List<OrderVM>> GetOrder(string? searchKeyWords, int status)
        {
            string storeId = _storeIdentityService.GetStoreId();
            TimeZoneInfo taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, taiwanTimeZone);
            await CheckUnconfirmedOrders();
            return await _context.Orders.AsNoTracking().Include(x => x.User).Include(x => x.OrderDetails).ThenInclude(x => x.Product)
                                                 .Where(c => c.StoreId == storeId &&
                                                 c.Status.StatusId != 5 &&
                                                 c.Status.StatusId != 6 &&
                                                 c.Status.StatusId != 7 &&
                                                 (status == 0 || c.Status.StatusId == status) &&
                                                 (string.IsNullOrEmpty(searchKeyWords) ||
                                                 c.OrderId.Contains(searchKeyWords) ||
                                                 c.User.UserName.ToLower().Contains(searchKeyWords.ToLower()) ||
                                                 c.OrderDetails.Any(od => od.Product.ProductName.ToLower().Contains(searchKeyWords.ToLower()))))
                 .Select(z => new OrderVM
                 {
                     OrderId = z.OrderId,
                     OrderDuration = (taiwanTime - z.OrderDate).TotalMinutes,
                     OrderDate = z.OrderDate.ToString("yyyy-MM-dd"),
                     OrderTime = z.OrderDate.ToString("HH:mm"),
                     DeliveryMethod = z.DeliveryMethod,
                     Address = z.Address,
                     Status = z.Status.StatusName,
                     StatusId = z.StatusId,
                     UserName = z.User.UserName,
                     UserEmail = z.User.UserEmail,
                     UserPhone = z.User.UserPhone,
                     PaymentMethodＮame = z.Payment.PaymentMethodＮame,
                     PaymentDate = z.Payment.PaymentTime.ToString("yyyy-MM-dd"),
                     PaymentTime = z.Payment.PaymentTime.ToString("HH:mm"),
                     OrderDetails = z.OrderDetails.Select(p => new OrderDetailsVM
                     {
                         PhotoPath = p.Product.PhotoPath,
                         ProductName = p.Product.ProductName,
                         UnitPrice = p.Product.UnitPrice,
                         Quantity = p.Quantity,
                         Total = p.Quantity * p.Product.UnitPrice,
                     }),
                     ShippingFee = z.ShippingFee,
                     Subtotal = z.OrderDetails.Sum(p => p.Quantity * p.Product.UnitPrice),
                     TotalQuantity = z.OrderDetails.Sum(p => p.Quantity),
                     DriverFullName = z.Driver.FullName
                 }).ToListAsync();
        }

        // PUT: api/StoreRealTimeOrdersapi/id
        [HttpPut("{id}")]
        public async Task<string> PutOrder(string id, [FromBody] RealTimeOrderVM realTimeOrderVM)
        {
            if (id != realTimeOrderVM.OrderId)
            {
                return "失敗";
            }
            Order? order = await _context.Orders.FindAsync(id);
            order.StatusId = realTimeOrderVM.StatusId;
            _context.Entry(order).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderExists(id))
                {
                    return "取消失敗";
                }
                else
                {
                    throw;
                }
            }
            return "已完成";

        }
        public async Task CheckUnconfirmedOrders()
        {
            TimeZoneInfo taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, taiwanTimeZone);
            var unconfirmedOrders = _context.Orders.Where(o => o.Status.StatusId == 1 && o.OrderDate.AddMinutes(15) < taiwanTime);
            foreach (Order? order in unconfirmedOrders)
            {
                order.StatusId = 7;
                _context.Entry(order).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        // POST: api/StoreRealTimeOrdersapi
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'Sifood3Context.Orders'  is null.");
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/StoreRealTimeOrdersapi/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> OrderExists(string id)
        {
            return _context.Orders != null && await _context.Orders.AnyAsync(e => e.OrderId == id);
        }
    }
}
