using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Admin.Models;
using sifoodproject.Models;

namespace sifoodproject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/OrderManage/{action}/{OrderId?}")]
    public class OrderManageController : Controller
    {
        private readonly Sifood3Context _context;
        public OrderManageController(Sifood3Context context)
        {
            _context = context;
        }
        // GET: Admin/OrderManage
        public async Task<IActionResult> Index()
        {
            var orderDetails = await _context.OrderDetails.Include(o => o.Order).ThenInclude(o => o.User).Include(o => o.Product).ToListAsync();
            var orderManageVMs = orderDetails
                .GroupBy(o => o.OrderId)
                .Select(group => new OrderManageVM
                {
                    UserName = group.First().Order.User.UserName,
                    OrderAddress = group.First().Order.Address,
                    ProductName = group.First().Product.ProductName,
                    Quantity = group.Sum(o => o.Quantity),
                    UserPhone = group.First().Order.User.UserPhone,
                    OrderDetailId = group.First().OrderDetailId,
                    OrderId = group.Key,
                    OrderDate = group.First().Order.OrderDate,
                    StatusName = group.First().Order.Status.StatusName,
                    ProductId = group.First().ProductId,
                    ProductUnitPrice = group.First().Product.UnitPrice,
                    StoreName = group.First().Order.Store.StoreName,
                    StorePhone = group.First().Order.Store.Phone,
                    StoreAddress = group.First().Order.Store.Address,
                    Total = group.Sum(o => o.Quantity * o.Product.UnitPrice),
                    OrderDetails = group.Select(detail => new OrderDetailVM
                    {
                        ProductId = detail.ProductId,
                        ProductName = detail.Product.ProductName,
                        Quantity = detail.Quantity
                    }).ToList()
                })
                .ToList();

            return View(orderManageVMs);
        }
        // GET: Admin/OrderManage/Details/5
        public async Task<IActionResult> Details(string? OrderId)
        {
            if (OrderId == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails
                .Where(x => x.OrderId == OrderId)
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Select(o => new OrderManageVM
                {
                    UserName = o.Order.User.UserName,
                    OrderAddress = o.Order.Address,
                    ProductName = o.Product.ProductName,
                    Quantity = o.Quantity,
                    UserPhone = o.Order.User.UserPhone,
                    OrderDetailId = o.OrderDetailId,
                    OrderId = o.OrderId,
                    OrderDate = o.Order.OrderDate,
                    StatusName = o.Order.Status.StatusName,
                    ProductId = o.ProductId,
                    ProductUnitPrice = o.Product.UnitPrice,
                    StoreName = o.Order.Store.StoreName,
                    StorePhone = o.Order.Store.Phone,
                    StoreAddress = o.Order.Store.Address,
                    Total = o.Quantity * o.Product.UnitPrice,
                })
                .ToListAsync();

            if (orderDetails == null || orderDetails.Count == 0)
            {
                return NotFound();
            }

            return View(orderDetails);
        }
        // GET: Admin/OrderManage/Edit/5
        public async Task<IActionResult> Edit(string? OrderId, int? ProductId)
        {
            if (OrderId == null || ProductId == null || _context.OrderDetails == null)
            {
                return NotFound();
            }
            var orderDetail = await _context.OrderDetails
                .Where(x => x.OrderId == OrderId && x.ProductId == ProductId)
                .Select(o => new OrderManageVM
                {
                    UserName = o.Order.User.UserName,
                    OrderAddress = o.Order.Address,
                    ProductName = o.Product.ProductName,
                    Quantity = o.Quantity,
                    UserPhone = o.Order.User.UserPhone,
                    OrderDetailId = o.OrderDetailId,
                    OrderId = o.OrderId,
                    OrderDate = o.Order.OrderDate,
                    StatusName = o.Order.Status.StatusName,
                    ProductId = o.ProductId,
                    ProductUnitPrice = o.Product.UnitPrice,
                    StoreName = o.Order.Store.StoreName,
                    StorePhone = o.Order.Store.Phone,
                    StoreAddress = o.Order.Store.Address,
                    Total = o.Quantity * o.Product.UnitPrice,
                })
                .FirstOrDefaultAsync();
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }
        // POST: Admin/OrderManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string OrderId, int ProductId, [Bind("OrderId,ProductId,UserName,OrderAddress,UserPhone")] OrderManageVM orderDetail)
        {
            if (OrderId != orderDetail.OrderId || ProductId != orderDetail.ProductId)
            {
                return NotFound();
            }
            var existingOrderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(x => x.OrderId == OrderId && x.ProductId == ProductId);
            if (existingOrderDetail == null)
            {
                return NotFound();
            }
            existingOrderDetail.Order.User.UserName = orderDetail.UserName;
            existingOrderDetail.Order.Address = orderDetail.OrderAddress;
            existingOrderDetail.Order.User.UserPhone = orderDetail.UserPhone;
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { OrderId = orderDetail.OrderId, ProductId = orderDetail.ProductId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(orderDetail.OrderId, orderDetail.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        // GET: Admin/OrderManage/Delete/5
        public async Task<IActionResult> Delete(string? OrderId, int? ProductId)
        {
            if (OrderId == null || ProductId == null || _context.OrderDetails == null)
            {
                return NotFound();
            }
            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Select(o => new OrderManageVM
                {
                    UserName = o.Order.User.UserName,
                    OrderAddress = o.Order.Address,
                    ProductName = o.Product.ProductName,
                    Quantity = o.Quantity,
                    UserPhone = o.Order.User.UserPhone,
                    OrderDetailId = o.OrderDetailId,
                    OrderId = o.OrderId,
                    OrderDate = o.Order.OrderDate,
                    StatusName = o.Order.Status.StatusName,
                    ProductId = o.ProductId,
                    ProductUnitPrice = o.Product.UnitPrice,
                    StoreName = o.Order.Store.StoreName,
                    StorePhone = o.Order.Store.Phone,
                    StoreAddress = o.Order.Store.Address,
                    Total = o.Quantity * o.Product.UnitPrice,
                })
                .FirstOrDefaultAsync(od => od.OrderId == OrderId && od.ProductId == ProductId);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }
        // POST: Admin/OrderManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string OrderId, int ProductId)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(OrderId, ProductId);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        private bool OrderDetailExists(string OrderId, int ProductId)
        {
            return (_context.OrderDetails?.Any(e => e.OrderId == OrderId && e.ProductId == ProductId)).GetValueOrDefault();
        }
    }
}
