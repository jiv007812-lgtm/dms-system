using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// 🔥 THÊM DATABASE
var connectionString = builder.Configuration.GetConnectionString("defaultconn");
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<DMS.Infrastructure.DataContext.DMSContext>(options =>
        options.UseNpgsql(connectionString));
    Console.WriteLine("✅ Database configured");
}

var app = builder.Build();
app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapGet("/", () => "DMS WITH DATABASE WORKS! ✅");
app.Run();