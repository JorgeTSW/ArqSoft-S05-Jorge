using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;
using CitasApp.Infrastructure.Observers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// CORS: permite que el front (file:// o Live Server) llame a esta API
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFront", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var entorno = builder.Environment.EnvironmentName;

// Factory + Decorator aplicados a PacienteRepository
builder.Services.AddScoped<IPacienteRepository>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    var repo = RepositoryFactory.CrearPacienteRepository(entorno, env);
    return new LoggingPacienteRepository(repo);
});

// Médicos y Citas — Factory sin Decorator por ahora
builder.Services.AddScoped<IMedicoRepository>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return RepositoryFactory.CrearMedicoRepository(entorno, env);
});

builder.Services.AddScoped<ICitaRepository>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return RepositoryFactory.CrearCitaRepository(entorno, env);
});

// Servicios de aplicación
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<MedicoService>();

// Registrar observers en CitaService
builder.Services.AddScoped<CitaService>(sp =>
{
    var repo = sp.GetRequiredService<ICitaRepository>();
    var service = new CitaService(repo);

    service.AgregarObserver(new SmsObserver());
    service.AgregarObserver(new EmailObserver());

    return service;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("PermitirFront");
app.UseAuthorization();
app.MapControllers();

app.Run();