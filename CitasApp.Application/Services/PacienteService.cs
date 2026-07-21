using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Application.Services
{
    // CAPA DE APLICACIÓN
    // Orquesta la lógica de negocio usando los Ports del Domain.
    // No sabe nada de JSON, archivos ni HTTP — solo trabaja con interfaces.
    public class PacienteService
    {
        private readonly IPacienteRepository _repo;

        public PacienteService(IPacienteRepository repo)
        {
            _repo = repo;
        }

        public List<Paciente> ObtenerTodos() => _repo.ObtenerTodos();

        public Paciente? ObtenerPorId(int id) => _repo.ObtenerPorId(id);
    }
}