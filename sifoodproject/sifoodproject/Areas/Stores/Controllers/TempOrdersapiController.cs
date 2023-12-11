
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Models;


namespace sifoodproject.Areas.Stores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempOrdersapiController : ControllerBase
    {
        private readonly Sifood3Context _context;

        public TempOrdersapiController(Sifood3Context context)
        {
            _context = context;
        }

        // GET: api/TempOrdersapi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/TempOrdersapi/5
        [HttpGet("{id}")]
        //public object GetOrder(string id)
        //{
        //    TimeZoneInfo taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
        //    DateTime utcNow = DateTime.UtcNow;
        //    DateTime taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, taiwanTimeZone);
        //    //List<int> StatusIdToCheck = new List<int> {1, 2, 3, 4};
        //    //var order = await _context.Orders.FindAsync(id);
        //    return _context.Orders.AsNoTracking().Include(x => x.User).Include(x => x.OrderDetails).ThenInclude(x => x.Product).Where(c => c.UserId == id && c.Status.StatusId != 5 && c.Status.StatusId != 6 && c.Status.StatusId != 7)
        //         .Select(z => new OrderVM
        //         {
        //             OrderId = z.OrderId,
        //             OrderDuration = (taiwanTime - z.OrderDate).TotalMinutes,
        //             OrderDate = z.OrderDate.ToString("yyyy-MM-dd"),
        //             OrderTime = z.OrderDate.ToString("HH:mm"),
        //             DeliveryMethod = z.DeliveryMethod,
        //             Address = z.Address,
        //             Status = z.Status.StatusName,
        //             StatusId = z.StatusId,
        //             UserName = z.User.UserName,
        //             UserEmail = z.User.UserEmail,
        //             UserPhone = z.User.UserPhone,
        //             PaymentMethodＮame = z.Payment.PaymentMethodＮame,
        //             PaymentDate = z.Payment.PaymentTime.ToString("yyyy-MM-dd"),
        //             PaymentTime = z.Payment.PaymentTime.ToString("HH:mm"),
        //             OrderDetails = z.OrderDetails.Select(p => new OrderDetailsVM
        //             {
        //                 PhotoPath = p.Product.PhotoPath,
        //                 ProductName = p.Product.ProductName,
        //                 UnitPrice = p.Product.UnitPrice,
        //                 Quantity = p.Quantity,
        //                 Total = p.Quantity * p.Product.UnitPrice,
        //             }),
        //             ShippingFee = (decimal)z.ShippingFee,
        //             Subtotal = z.OrderDetails.Sum(p => p.Quantity * p.Product.UnitPrice),
        //             TotalQuantity = z.OrderDetails.Sum(p => p.Quantity),
        //             DriverFullName = z.Driver.FullName
        //         });
        //}
        // PUT: api/TempOrdersapi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(string id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TempOrdersapi
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

        // DELETE: api/TempOrdersapi/5
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
    }
}
