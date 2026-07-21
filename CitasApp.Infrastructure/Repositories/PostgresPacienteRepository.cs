using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using Npgsql;

namespace CitasApp.Infrastructure.Repositories
{
    public class PostgresPacienteRepository : IPacienteRepository
    {
        private readonly string _connectionString;

        public PostgresPacienteRepository(string connectionString)
        {
            _connectionString = connectionString;
            InicializarTabla();
        }

        private void InicializarTabla()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Pacientes (
                    Id       SERIAL PRIMARY KEY,
                    Nombre   VARCHAR(100) NOT NULL,
                    Apellido VARCHAR(100) NOT NULL,
                    Email    VARCHAR(150),
                    Telefono VARCHAR(50)
                );";
            cmd.ExecuteNonQuery();
        }

        private static Paciente LeerFila(NpgsqlDataReader r) => new Paciente
        {
            Id = r.GetInt32(0),
            Nombre = r.GetString(1),
            Apellido = r.GetString(2),
            Email = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            Telefono = r.IsDBNull(4) ? string.Empty : r.GetString(4)
        };

        public List<Paciente> ObtenerTodos()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT Id, Nombre, Apellido, Email, Telefono FROM Pacientes;";

            var lista = new List<Paciente>();
            using var r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(LeerFila(r));
            return lista;
        }

        public Paciente? ObtenerPorId(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT Id, Nombre, Apellido, Email, Telefono " +
                "FROM Pacientes WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            return r.Read() ? LeerFila(r) : null;
        }
    }
}