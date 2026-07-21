using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using Npgsql;

namespace CitasApp.Infrastructure.Repositories
{
    public class PostgresMedicoRepository : IMedicoRepository
    {
        private readonly string _connectionString;

        public PostgresMedicoRepository(string connectionString)
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
                CREATE TABLE IF NOT EXISTS Medicos (
                    Id             SERIAL PRIMARY KEY,
                    Nombre         VARCHAR(100) NOT NULL,
                    Apellido       VARCHAR(100) NOT NULL,
                    Especialidad   VARCHAR(150),
                    NumeroLicencia VARCHAR(50)
                );";
            cmd.ExecuteNonQuery();
        }

        private static Medico LeerFila(NpgsqlDataReader r) => new Medico
        {
            Id = r.GetInt32(0),
            Nombre = r.GetString(1),
            Apellido = r.GetString(2),
            Especialidad = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            NumeroLicencia = r.IsDBNull(4) ? string.Empty : r.GetString(4)
        };

        public List<Medico> ObtenerTodos()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT Id, Nombre, Apellido, Especialidad, NumeroLicencia FROM Medicos;";

            var lista = new List<Medico>();
            using var r = cmd.ExecuteReader();
            while (r.Read()) lista.Add(LeerFila(r));
            return lista;
        }

        public Medico? ObtenerPorId(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "SELECT Id, Nombre, Apellido, Especialidad, NumeroLicencia " +
                "FROM Medicos WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            return r.Read() ? LeerFila(r) : null;
        }
    }
}