using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Application.Services
{
    public class CitaService
    {
        private readonly ICitaRepository _repo;
        private readonly List<ICitaObserver> _observers = new();

        public CitaService(ICitaRepository repo)
        {
            _repo = repo;
        }

        // Registrar observers desde fuera — Domain no conoce las implementaciones
        public void AgregarObserver(ICitaObserver observer)
            => _observers.Add(observer);

        public List<Cita> ObtenerTodos() => _repo.ObtenerTodos();

        public List<Cita> ObtenerPorPaciente(int pacienteId)
            => _repo.ObtenerPorPaciente(pacienteId);

        public void ConfirmarCita(Cita cita)
        {
            cita.Estado = "Confirmada";

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Cita {cita.Id} confirmada");

            // Notificar a todos los observers registrados
            foreach (var observer in _observers)
            {
                try
                {
                    observer.OnCitaConfirmada(cita);
                }
                catch (Exception ex)
                {
                    // Circuit Breaker básico — si un observer falla, los demás siguen
                    Console.WriteLine($"[WARNING] Observer falló: {ex.Message}");
                }
            }
        }
    }
}