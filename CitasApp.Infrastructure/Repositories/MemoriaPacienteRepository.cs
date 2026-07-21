using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Repositories
{
    public class MemoriaPacienteRepository : IPacienteRepository
    {
        private readonly List<Paciente> _pacientes = new()
        {
            new Paciente { Id = 1, Nombre = "Carlos",   Apellido = "Ramírez",  Email = "carlos@mail.com",  Telefono = "555-1001" },
            new Paciente { Id = 2, Nombre = "Sofía",    Apellido = "Torres",   Email = "sofia@mail.com",   Telefono = "555-1002" },
            new Paciente { Id = 3, Nombre = "Fernando", Apellido = "Castillo", Email = "fernando@mail.com",Telefono = "555-1003" },
        };

        public List<Paciente> ObtenerTodos() => _pacientes;

        public Paciente? ObtenerPorId(int id) =>
            _pacientes.FirstOrDefault(p => p.Id == id);
    }
}