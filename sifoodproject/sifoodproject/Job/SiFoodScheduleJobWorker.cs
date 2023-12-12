using Hangfire;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Models;

namespace sifoodproject.Job
{
    public class SiFoodScheduleJobWorker
    {
        public readonly IBackgroundJobClient _backgroundJobClient;
        public readonly Sifood3Context _sifood3Context;

        public SiFoodScheduleJobWorker(IBackgroundJobClient backgroundJobClient, Sifood3Context sifood3Context)
        {
            _backgroundJobClient = backgroundJobClient;
            _sifood3Context = sifood3Context;
        }

        public void ExecuteAllJob()
        {
            RecurringJob.AddOrUpdate("CheckUnconfirmedOrdersJob", () => CheckUnconfirmedOrders(), "*/1 * * * *", TimeZoneInfo.Local);
        }

        public void CheckUnconfirmedOrders()
        {
            TimeZoneInfo taiwanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime utcNow = DateTime.UtcNow;
            DateTime taiwanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, taiwanTimeZone);
            var unconfirmedOrders = _sifood3Context.Orders.Where(o => o.Status.StatusId == 1 && o.OrderDate.AddMinutes(15) < taiwanTime);
            foreach (Order? order in unconfirmedOrders)
            {
                order.StatusId = 7;
                _sifood3Context.Entry(order).State = EntityState.Modified;
            }
            _sifood3Context.SaveChanges();
        }
    }
}
