using HealthcareApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using HealthcareApp.Components;
using HealthcareApp.Services;
using MudBlazor.Services;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);

// 1. Load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// 2. Disable IPv6 if needed (optional)
AppContext.SetSwitch("System.Net.Sockets.Http.UseIPv6", false);

// 3. Register the database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("HealthcareApp"));
});

// 4. Add Controllers (if you have API endpoints)
builder.Services.AddControllers();

// 5. Add Razor Pages (if you serve any traditional Razor Pages)
builder.Services.AddRazorPages();

// 6. Add Server-Side Blazor with .NET 8 "interactive server components"
builder.Services.AddServerSideBlazor()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
// 7. Add a named HttpClient if you need to call an external API
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:7035/api/");
});
// Register VertexAiService for DI
builder.Services.AddSingleton<VertexAiChatService>();
builder.Services.AddSingleton<PythonApiService>();
// 8. Build the application
var app = builder.Build();

// 9. Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// If you want to force HTTPS
app.UseHttpsRedirection();

// Serve static files (CSS, images, JS, etc.)
app.UseStaticFiles();

// Optionally use Antiforgery if your controllers/pages need it
app.UseAntiforgery();

// Map your controllers (if you have an API or MVC controllers)
app.MapControllers();

// 10. Map Razor Components with interactive server render mode
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 11. Run the app
app.Run();