using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CitasApp.Infrastructure.Data
{
    /// <summary>
    /// Contexto de datos de la infraestructura.
    /// Al heredar de IdentityDbContext, automáticamente incluye todas las tablas de Identity
    /// necesarias para gestionar usuarios, roles, contraseñas y claims en PostgreSQL.
    /// </summary>
    public class CitasDbContext : IdentityDbContext<IdentityUser>
    {
        public CitasDbContext(DbContextOptions<CitasDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ¡CRÍTICO! Llama a base.OnModelCreating antes de tus configuraciones personalizadas.
            // Si omites esto, la configuración interna de las tablas de ASP.NET Identity fallará estrepitosamente.
            base.OnModelCreating(builder);

            // ============================================================================
            // CONFIGURACIONES ADICIONALES (Opcional)
            // ============================================================================
            // Aquí puedes personalizar los nombres de las tablas de Identity si prefieres 
            // usar minúsculas o nombres en español para adaptarlos mejor a PostgreSQL:

            builder.Entity<IdentityUser>(entity => entity.ToTable("Usuarios"));
            builder.Entity<IdentityRole>(entity => entity.ToTable("Roles"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UsuarioRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UsuarioClaims"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UsuarioLogins"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UsuarioTokens"));

            // Si en el futuro decides migrar tus entidades de negocio (Paciente, Medico, Cita) 
            // desde tus archivos JSON hacia PostgreSQL, simplemente descomenta estas líneas:
            // builder.Entity<Paciente>().ToTable("Pacientes");
            // builder.Entity<Medico>().ToTable("Medicos");
            // builder.Entity<Cita>().ToTable("Citas");
        }
    }
}