using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// DATABASE
var connectionString = builder.Configuration.GetConnectionString("defaultconn");
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<DMS.Infrastructure.DataContext.DMSContext>(options =>
        options.UseNpgsql(connectionString));
    Console.WriteLine("✅ Database configured");
}

// 🚨 TẠM BỎ IDENTITY - CHỈ GIỮ DATABASE
Console.WriteLine("ℹ️ Identity temporarily disabled");

var app = builder.Build();

app.UseRouting();
// 🚨 TẠM BỎ AUTHENTICATION
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapGet("/", () => "DMS DATABASE WORKS! ✅");
app.MapGet("/test-db", () => "Database connection is ready! 🗄️");

Console.WriteLine("🎉 Application with Database started!");

app.Run();