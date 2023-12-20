using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Admin.NewFolder;
using sifoodproject.Areas.Users.Hubs;
using sifoodproject.Models;
using sifoodproject.Services;


namespace sifoodproject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddResponseCaching();
            builder.Services.AddDbContextPool<Sifood3Context>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Sifood"));
            });

            builder.Services.AddHangfire(configuration => configuration
                           .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                           .UseSimpleAssemblyNameTypeSerializer()
                           .UseRecommendedSerializerSettings()
                           .UseSqlServerStorage(builder.Configuration.GetConnectionString("Sifood")));
            builder.Services.AddHangfireServer();

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IUserIdentityService, UserIdentityService>();
            builder.Services.AddTransient<IStoreIdentityService, StoreIdentityService>();
            builder.Services.AddTransient<IAdminService, AdminIdentityService>();
            builder.Services.AddTransient<AdminChatHub>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Users/Account/LoginRegister";
                options.AccessDeniedPath = "/Account/LoginRegister";
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        var loginPath = "/Users/Account/LoginRegister";
                        context.Response.Redirect(loginPath);
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddSignalR();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area=Users}/{controller=Home}/{action=Main}/{id?}"
            );
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<AdminChatHub>("/adminchatHub");
            app.Run();
        }
    }
}
