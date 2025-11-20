using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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

// 🔥 THÊM IDENTITY
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

app.UseRouting();
app.UseAuthentication();  // 🔥 THÊM DÒNG NÀY
app.UseAuthorization();   // 🔥 THÊM DÒNG NÀY

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapGet("/", () => "DMS WITH IDENTITY WORKS! ✅");
app.MapGet("/test-auth", () => "Authentication is working! 🔐");

Console.WriteLine("🎉 Application with Identity started!");

app.Run();