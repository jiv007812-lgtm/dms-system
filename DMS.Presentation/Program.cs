using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DMS.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls("http://0.0.0.0:10000");

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 🔥 CHỈ DÙNG POSTGRESQL - KHÔNG CÓ SQL SERVER
            var connectionString = builder.Configuration.GetConnectionString("defaultconn");

            if (!string.IsNullOrEmpty(connectionString))
            {
                // LUÔN LUÔN DÙNG POSTGRESQL
                builder.Services.AddDbContext<DMSContext>(options =>
                    options.UseLazyLoadingProxies().UseNpgsql(connectionString));
                Console.WriteLine("✅ Using PostgreSQL database");
            }
            else
            {
                Console.WriteLine("❌ No database connection string found");
            }

            // 🔥 SỬA LỖI APPLICATIONUSER - DÙNG IDENTITYUSER
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<DMSContext>()
            .AddDefaultTokenProviders();

            // 🔥 ĐĂNG KÝ TRỰC TIẾP CLASS - KHÔNG DÙNG INTERFACE
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<AccountService>(); 
            builder.Services.AddScoped<DocumentService>();
            
            // ✅ GIẢI PHÁP CHO LỖI MAPPERPROFILE
            // Cách 1: Sử dụng Assembly - sẽ tìm tất cả Profile trong tất cả referenced assemblies
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            
            // Hoặc Cách 2: Nếu biết chính xác assembly chứa MapperProfile
            // builder.Services.AddAutoMapper(typeof(AccountService).Assembly);

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}