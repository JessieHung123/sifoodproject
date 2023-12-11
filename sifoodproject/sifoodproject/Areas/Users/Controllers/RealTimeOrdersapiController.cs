using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject._0.Areas.Users.Models.ViewModels;
using sifoodproject.Areas.Users.Models.ViewModels;
using sifoodproject.Models;
using sifoodproject.Services;

namespace SiFoodProjectFormal2._0.Areas.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealTimeOrdersapiController : ControllerBase
    {
        private readonly Sifood3Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserIdentityService _userIdentityService;

        public RealTimeOrdersapiController(Sifood3Context context, IWebHostEnvironment webHostEnvironment, IUserIdentityService userIdentityService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userIdentityService = userIdentityService;
        }

        // GET: api/RealTimeOrdersapi
        //[HttpGet]

        [HttpGet]
        public async Task<List<OrderVM>> GetOrder()
        {
            string userId = _userIdentityService.GetUserId();
            TimeZoneInfo taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, taiwanTimeZone);
            return await _context.Orders.AsNoTracking().Include(x => x.User).Include(x => x.OrderDetails).ThenInclude(x => x.Product).Where(c => c.UserId == userId && c.Status.StatusId != 5 && c.Status.StatusId != 6 && c.Status.StatusId != 7)
                 .Select(z => new OrderVM
                 {
                     OrderId = z.OrderId,
                     OrderDuration = (taiwanTime - z.OrderDate).TotalMinutes,
                     OrderDateTime = z.OrderDate,
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
                     ShippingFee = (decimal)z.ShippingFee,
                     Subtotal = z.OrderDetails.Sum(p => p.Quantity * p.Product.UnitPrice),
                     TotalQuantity = z.OrderDetails.Sum(p => p.Quantity),
                     DriverFullName = z.Driver.FullName
                 }).ToListAsync();
        }

        // PUT: api/RealTimeOrdersapi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                if (!OrderExists(id))
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

        // POST: api/RealTimeOrdersapi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // DELETE: api/RealTimeOrdersapi/5
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

        private bool OrderExists(string id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
        [HttpGet("DownloadOrderDetails/{orderId}")]
        public async Task<IActionResult> DownloadOrderDetails(string orderId)
        {
            var order = await _context.Orders.Include(o => o.User)
                                             .Include(o => o.OrderDetails)
                                             .ThenInclude(od => od.Product)
                                             .Include(o => o.Payment)
                                             .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return Content("無法下載");
            }


            var orderDetailsContent = "商品,單價,數量,總計\n";
            foreach (var orderDetail in order.OrderDetails)
            {
                orderDetailsContent += $"{orderDetail.Product?.ProductName ?? "N/A"},{orderDetail.Product?.UnitPrice ?? 0},{orderDetail.Quantity},{orderDetail.Quantity * (orderDetail.Product?.UnitPrice ?? 0)}\n";
            }

            orderDetailsContent += $"\n訂單編號: {order.OrderId}";
            orderDetailsContent += $"\n訂購日期: {order.OrderDate}";
            orderDetailsContent += $"\n取餐方式: {order.DeliveryMethod}";
            orderDetailsContent += $"\n收件地址: {order.Address ?? "N/A"}";
            orderDetailsContent += $"\n訂單狀態: {order.Status.StatusName}";
            orderDetailsContent += $"\n顧客姓名: {order.User.UserName ?? "N/A"}";
            orderDetailsContent += $"\n顧客電子郵件: {order.User.UserEmail ?? "N/A"}";
            orderDetailsContent += $"\n顧客手機號碼: {order.User.UserPhone ?? "N/A"}";
            orderDetailsContent += $"\n付款方式: {order.Payment?.PaymentMethodＮame ?? "N/A"}";
            orderDetailsContent += $"\n付款日期: {order.Payment?.PaymentTime}";
            orderDetailsContent += $"\n運費: {order.ShippingFee ?? 0}";
            orderDetailsContent += $"\n總額: {order.OrderDetails.Sum(p => p.Quantity * p.Product.UnitPrice) + (order.DeliveryMethod == "外送" ? order.ShippingFee : 0)}";

            var fileBytes = Encoding.UTF8.GetBytes(orderDetailsContent);
            var fileName = $"OrderDetails_{orderId}.csv";

            return File(fileBytes, "text/csv", fileName);
        }

    }
}
