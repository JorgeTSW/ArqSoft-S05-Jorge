using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;
using CitasApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load the .env file variables into the application context
var appEnv = builder.Configuration["APP_ENVIRONMENT"]?.ToLower() ?? "dev";
Console.WriteLine(appEnv);

/* ============================================================================
 * DYNAMIC INJECTION OF DEPENDENCIES — Ports & Adapters
 * ============================================================================ */
if (appEnv == "prod")
{
    // 🟢 PRODUCTION MODE: Pulls structural data dynamically via PostgreSQL ADO.NET Repositories
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddScoped<IPacienteRepository>(sp => new PostgresPacienteRepository(connectionString));
    builder.Services.AddScoped<IMedicoRepository>(sp => new PostgresMedicoRepository(connectionString));
    builder.Services.AddScoped<ICitaRepository>(sp => new PostgresCitaRepository(connectionString));
}
else
{
    // 🟡 DEVELOPMENT MODE: Fallback default reads local flat data out of /data JSON files
    builder.Services.AddScoped<IPacienteRepository, JsonPacienteRepository>();
    builder.Services.AddScoped<IMedicoRepository, JsonMedicoRepository>();
    builder.Services.AddScoped<ICitaRepository, JsonCitaRepository>();
}

// SERVICIOS DE APLICACIÓN: Ahora consumen las interfaces inyectadas dinámicamente arriba
builder.Services.AddScoped<PacienteService>(sp =>
    new PacienteService(sp.GetRequiredService<IPacienteRepository>()));

builder.Services.AddScoped<MedicoService>(sp =>
    new MedicoService(sp.GetRequiredService<IMedicoRepository>()));

builder.Services.AddScoped<CitaService>(sp =>
    new CitaService(sp.GetRequiredService<ICitaRepository>()));

builder.Services.AddControllersWithViews();

/* ============================================================================
 * DATABASES & IDENTITY INFRASTRUCTURE (PostgreSQL Setup)
 * ============================================================================ */
var mainConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CitasDbContext>(options =>
    options.UseNpgsql(
        mainConnectionString,
        b => b.MigrationsAssembly("CitasApp.Infrastructure")
    ));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<CitasDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

/* ============================================================================ */
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();