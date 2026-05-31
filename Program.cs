using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/* ============================================================================
 * INYECCIÓN DE DEPENDENCIAS — Ports & Adapters
 * ============================================================================
 * 
 * Aquí decidimos qué implementación (Adapter) se usa para cada interfaz (Port).
 * Cambiando una sola línea, toda la app usa una fuente de datos diferente
 * sin tocar controllers, vistas ni lógica de negocio.
 * 
 * */

// Adapter JSON — lee datos desde archivos en /data
builder.Services.AddScoped<IPacienteRepository, JsonPacienteRepository>();

// Adapter Memoria — datos hardcodeados en memoria
//builder.Services.AddScoped<IPacienteRepository, MemoriaPacienteRepository>();

// ============================================================================

builder.Services.AddScoped<IMedicoRepository, JsonMedicoRepository>();
builder.Services.AddScoped<ICitaRepository, JsonCitaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
