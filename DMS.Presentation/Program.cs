var builder = WebApplication.CreateBuilder(args);

// Services cơ bản
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

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