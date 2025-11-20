using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 🔥 PORT RENDER
builder.WebHost.UseUrls("http://*:" + (Environment.GetEnvironmentVariable("PORT") ?? "10000"));

// Services cơ bản
builder.Services.AddControllersWithViews();

// 🔥 DATABASE POSTGRESQL CHO RENDER
var connectionString = builder.Configuration.GetConnectionString("defaultconn");
Console.WriteLine($"🔍 Connection String: {connectionString}");

if (!string.IsNullOrEmpty(connectionString))
{
    try
    {
        builder.Services.AddDbContext<DMS.Infrastructure.DataContext.DMSContext>(options =>
            options.UseNpgsql(connectionString));
        Console.WriteLine("✅ PostgreSQL database configured");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database error: {ex.Message}");
    }
}

// 🔥 IDENTITY ĐƠN GIẢN - DÙNG IdentityUser
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<DMS.Infrastructure.DataContext.DMSContext>();

Console.WriteLine("✅ Identity configured");

var app = builder.Build();

// LUÔN HIỆN LỖI CHI TIẾT
app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ROUTING ĐƠN GIẢN
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// TEST ENDPOINTS
app.MapGet("/", () => "DMS SYSTEM IS WORKING! ✅");
app.MapGet("/test", () => "TEST OK! 🎉");
app.MapGet("/health", () => new { status = "OK", time = DateTime.Now });

Console.WriteLine("🎉 Application started successfully!");

app.Run();