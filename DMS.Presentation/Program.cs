using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.MapperHelper;
using DMS.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
// 🔥 THÊM USING DIRECTIVES CHO POSTGRESQL
using Npgsql.EntityFrameworkCore.PostgreSQL;

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

            // 🔥 SỬA LẠI - CHỈ ĐĂNG KÝ 1 DATABASE PROVIDER
            var connectionString = builder.Configuration.GetConnectionString("defaultconn");
            
            if (!string.IsNullOrEmpty(connectionString) && 
               (connectionString.Contains("PostgreSQL") || connectionString.Contains("postgres")))
            {
                // CHỈ DÙNG POSTGRESQL - KHÔNG CÓ ELSE
                builder.Services.AddDbContext<DMSContext>(options =>
                    options.UseLazyLoadingProxies().UseNpgsql(connectionString));
                Console.WriteLine("Using PostgreSQL database");
            }
            else
            {
                // KHÔNG ĐĂNG KÝ DATABASE PROVIDER NÀO CẢ
                // ĐỂ TRÁNH CONFLICT
                Console.WriteLine("No database provider registered - using in-memory");
            }

            builder.Services.AddAutoMapper(op => op.AddProfile(typeof(MappingProfile)));

            builder.Services.AddIdentity<AppUser, IdentityRole>(op=>
            {
                op.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<DMSContext>().AddDefaultTokenProviders();

            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IFolderService, FolderService>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();
            builder.Services.AddScoped<ISharingService, SharingService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDashBoardService, DashBoardService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";
            });

            builder.Services.AddScoped<ITrashService, TrashService>();        
            builder.Services.AddScoped<IStarredService, StarredService>();        

            var app = builder.Build();

            // 🔧 TỰ ĐỘNG MIGRATE DATABASE - CHỈ KHI CÓ DATABASE
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DMSContext>();
                    context.Database.Migrate();
                    Console.WriteLine("Database migrated successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database migration: {ex.Message}");
                }
            }
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

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