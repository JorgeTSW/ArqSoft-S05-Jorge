using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Observers
{
    public class SmsObserver : ICitaObserver
    {
        public void OnCitaConfirmada(Cita cita)
        {
            Console.WriteLine($"[SMS] Recordatorio enviado al paciente {cita.PacienteId} " +
                              $"— cita el {cita.Fecha:dd/MM/yyyy} a las {cita.Hora:HH:mm}");
        }
    }
}