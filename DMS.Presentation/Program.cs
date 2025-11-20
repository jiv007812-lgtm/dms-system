using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 🔥 QUAN TRỌNG: Port Render
builder.WebHost.UseUrls("http://*:" + (Environment.GetEnvironmentVariable("PORT") ?? "10000"));

// Services cơ bản
builder.Services.AddControllersWithViews();

// 🔥 DATABASE - SỬA NAMESPACE ĐÚNG
var connectionString = builder.Configuration.GetConnectionString("defaultconn");
Console.WriteLine($"🔍 Connection String: {connectionString}");

if (!string.IsNullOrEmpty(connectionString))
{
    // SỬA NAMESPACE: DMS.Infrastructure.DataContext
    builder.Services.AddDbContext<DMS.Infrastructure.DataContext.DMSContext>(options =>
        options.UseNpgsql(connectionString));
    Console.WriteLine("✅ PostgreSQL database configured");
}
else
{
    Console.WriteLine("❌ No connection string found");
}

// 🔥 IDENTITY - SỬA NAMESPACE ĐÚNG
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

var app = builder.Build();

// 🔥 LUÔN HIỆN LỖI CHI TIẾT
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 🔥 ROUTING ĐƠN GIẢN
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔥 TEST ENDPOINT
app.MapGet("/", () => "DMS Application is running! ✅");
app.MapGet("/test", () => Results.Json(new { status = "OK", message = "Server is working" }));
app.MapGet("/health", () => Results.Json(new { status = "Healthy", timestamp = DateTime.UtcNow }));

Console.WriteLine("🎉 Application started successfully on Render!");

app.Run();