using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Observers
{
    public class EmailObserver : ICitaObserver
    {
        public void OnCitaConfirmada(Cita cita)
        {
            Console.WriteLine($"[EMAIL] Confirmación enviada al paciente {cita.PacienteId} " +
                              $"— motivo: {cita.Motivo} — estado: {cita.Estado}");
        }
    }
}