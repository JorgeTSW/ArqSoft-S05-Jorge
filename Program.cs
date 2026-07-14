using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;
// 1. ADD THESE NEW INITIAL IMPORTS FOR EF CORE & IDENTITY
using CitasApp.Infrastructure.Data; // Replace with your actual namespace where CitasDbContext lives
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Servicios de aplicación
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<CitaService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

/* ============================================================================
 * NEW: DATABASES & IDENTITY INFRASTRUCTURE (PostgreSQL Setup)
 * ============================================================================ */
// Extract the connection string from your appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register your DbContext to use Npgsql (PostgreSQL)
builder.Services.AddDbContext<CitasDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        b => b.MigrationsAssembly("CitasApp.Infrastructure") // Points migrations to the Infrastructure project
    ));

// Configure ASP.NET Core Identity services for login management
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Example password rules (Adjust these as you prefer)
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<CitasDbContext>()
    .AddDefaultTokenProviders();

// Configure Application Cookie settings for login redirects
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Where users are sent if not authenticated
    options.AccessDeniedPath = "/Account/AccessDenied";
});
/* ============================================================================ */


/* ============================================================================
 * INYECCIÓN DE DEPENDENCIAS — Ports & Adapters
 * ============================================================================ */

// Adapter JSON — lee datos desde archivos en /data
builder.Services.AddScoped<IPacienteRepository, JsonPacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, JsonMedicoRepository>();
builder.Services.AddScoped<ICitaRepository, JsonCitaRepository>();

// ============================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// 2. IMPORTANT CONFIGURATION ORDER: Authentication MUST come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();