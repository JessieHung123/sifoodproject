namespace sifoodproject.Job
{
    public static class SiFoodScheduleJobExtenstion
    {
        public static IServiceCollection AddSiFoodScheduleJob(this IServiceCollection services)
        {
            services.AddScoped<SiFoodScheduleJobWorker, SiFoodScheduleJobWorker>();

            return services;
        }
        public static WebApplication UseSiFoodDailyJob(this WebApplication app)
        {
            using (var sc = app.Services.CreateScope())
            {
                var worker = sc.ServiceProvider.GetRequiredService<SiFoodScheduleJobWorker>();

                worker.ExecuteAllJob();

                return app;
            }
        }
    }
}
