using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Drivers.Models;
using sifoodproject.Models;

namespace sifoodproject.Areas.Drivers.Controllers
{
    [Route("api/DeliveryOrderapi/[action]")]

    [Area("Drivers")]
    public class DeliveryOrderapiController:Controller
    {
        private readonly Sifood3Context _context;

        public DeliveryOrderapiController(Sifood3Context context)
        {
            _context = context;

        }
        public object WaitForDeliveryOrderSimple()
        {
            return _context.Orders.Where(o => o.StatusId == 2 && o.DeliveryMethod=="外送" && o.OrderDate.Date == DateTime.Today).Include(o => o.User).Include(o => o.Store).Select(o => new DeliveryOrderVM
            {
                OrderId = o.OrderId,
                Address = o.Address,
                StoreName = o.Store.StoreName,
                StoreAddress=o.Store.Address,
                UserName = o.User.UserName==null? "": o.User.UserName,
                Latitude=(decimal)o.Store.Latitude,
                Longitude= (decimal)o.Store.Longitude,
                StatusId = o.StatusId,
                DriverId=o.DriverId==null?"D001": o.DriverId,
            });
        }
        [HttpGet("{id}")]
        public object WaitForDeliveryOrderDetails(string id)
        {
            return _context.Orders.Where(o=>o.OrderId==id&&o.OrderDate.Date == DateTime.Today && o.DeliveryMethod == "外送"&&( o.StatusId == 2|| o.StatusId == 4)).Include(o => o.User).Include(o => o.Store).Include(x => x.OrderDetails).ThenInclude(x => x.Product).Select(o => new ChooseOrderVM
            {
                OrderId = o.OrderId,
                StatusId=o.StatusId,
                Address = o.Address,
                StoreName = o.Store.StoreName,
                StoreAddress = o.Store.Address,
                Latitude = (decimal)o.Store.Latitude,
                Longitude = (decimal)o.Store.Longitude,
                UserName = o.User.UserName == null ? "NotFind" : o.User.UserName,
                UserPhone = o.User.UserPhone == null ? "NotFind" : o.User.UserPhone,
                OrderDetails = o.OrderDetails.Select(p => new OrderDetailsVM
                {
                    ProductName = p.Product.ProductName,
                    Quantity = p.Quantity,
                }),
                DriverId = o.DriverId == null ? "D001" : o.DriverId,
            });
        }
        [HttpPut("{orderId}")]
        public async Task<bool> UpdateOrderStatus(string orderId)
        {
            Order? order = await _context.Orders.FindAsync(orderId); ;
            order.StatusId = 4;
            order.DriverId = "D001";
            _context.Entry(order).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception) { return false; }
        }
        public object OnTheWayOrder()
        {
            return _context.Orders.Where(o => o.StatusId == 4 && o.DeliveryMethod == "外送" && o.OrderDate.Date == DateTime.Today).Include(o => o.User).Include(o => o.Store).Select(o => new DeliveryOrderVM
            {
                OrderId = o.OrderId,
                Address = o.Address,
                StoreName = o.Store.StoreName,
                StoreAddress = o.Store.Address,
                UserName = o.User.UserName == null ? "NotFind" : o.User.UserName,
                StatusId = o.StatusId,
                UserPhone = o.User.UserPhone == null ? "NotFind" : o.User.UserPhone,
                OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                OrderTime=o.OrderDate.ToShortTimeString(),
                DriverId = o.DriverId == null ? "D001" : o.DriverId,
            }) ;
        }
        [HttpPut("{orderId}")]
        public async Task<bool> FinishOrder(string orderId)
        {
            Order? order = await _context.Orders.FindAsync(orderId); ;
            order.StatusId = 5;
            _context.Entry(order).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
