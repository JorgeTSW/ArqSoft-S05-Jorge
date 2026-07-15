using CitasApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Web.Controllers
{
    [Authorize]
    public class PacienteController : Controller
    {
        private readonly PacienteService _pacienteService;

        public PacienteController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: /Paciente o /Paciente/Index
        public IActionResult Index()
        {
            var usuarioEmail = User.Identity?.Name;

            // 1. REGLA ESPECIAL: Si es el administrador, ve el catálogo completo
            if (usuarioEmail == "jorge@admin.com")
            {
                return View(_pacienteService.ObtenerTodos());
            }

            // 2. Si es un paciente, buscamos su registro por su correo electrónico
            var paciente = _pacienteService.ObtenerTodos()
                .FirstOrDefault(p => p.Email == usuarioEmail);

            if (paciente == null)
            {
                // Si la cuenta logueada no existe en la tabla de pacientes, redirige a Home
                return RedirectToAction("Index", "Home");
            }

            // 3. Redirección automática a la vista de Detalles de este paciente específico
            return RedirectToAction("Detalle", new { id = paciente.Id });
        }

        // GET: /Paciente/Detalle/5
        public IActionResult Detalle(int id)
        {
            var usuarioEmail = User.Identity?.Name;
            var paciente = _pacienteService.ObtenerPorId(id);

            if (paciente == null)
            {
                return NotFound();
            }

            // Medida de Seguridad Extra: Evitar que un paciente intente cambiar el ID en la URL 
            // para espiar los detalles de otro paciente.
            if (usuarioEmail != "jorge@admin.com" && paciente.Email != usuarioEmail)
            {
                return Forbid(); // Acceso denegado
            }

            return View(paciente);
        }
    }
}