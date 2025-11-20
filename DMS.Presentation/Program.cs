var builder = WebApplication.CreateBuilder(args);

// Services tối thiểu
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Basic configuration
app.UseRouting();

// Simple routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Test endpoints
app.MapGet("/", () => "DMS SYSTEM IS WORKING! ✅");
app.MapGet("/test", () => "TEST ENDPOINT WORKS! 🎉");
app.MapGet("/health", () => new { status = "OK", time = DateTime.Now });

Console.WriteLine("🚀 Application started!");

app.Run();