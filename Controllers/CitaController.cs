using CitasApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Web.Controllers
{
    [Authorize]
    public class CitaController : Controller
    {
        private readonly CitaService _citaService;
        private readonly PacienteService _pacienteService;
        private readonly MedicoService _medicoService;

        public CitaController(CitaService citaService,
                              PacienteService pacienteService,
                              MedicoService medicoService)
        {
            _citaService = citaService;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        public IActionResult Index()
        {
            // 1. Obtener el email del usuario logueado en Identity (prueba)
            var usuarioEmail = User.Identity?.Name;

            // Cargar los catálogos en el ViewBag para las vistas (combos, tablas, etc.)
            ViewBag.Pacientes = _pacienteService.ObtenerTodos();
            ViewBag.Medicos = _medicoService.ObtenerTodos();

            // 2. REGLA ESPECIAL: Si es el administrador, mostrar TODO
            if (usuarioEmail == "jorge@admin.com")
            {
                return View(_citaService.ObtenerTodos());
            }

            // 3. Si no es admin, buscar al paciente en la base de datos a través de su email
            var paciente = _pacienteService.ObtenerTodos()
                .FirstOrDefault(p => p.Email == usuarioEmail);

            if (paciente == null)
            {
                // Si no se encuentra (y tampoco es el admin registrado), enviamos una lista vacía
                return View(new List<CitasApp.Domain.Models.Cita>());
            }

            // 4. Filtrar las citas específicas de este paciente
            return View(_citaService.ObtenerPorPaciente(paciente.Id));
        }

        public IActionResult PorPaciente(int pacienteId)
        {
            ViewBag.Pacientes = _pacienteService.ObtenerTodos();
            ViewBag.Medicos = _medicoService.ObtenerTodos();
            return View(_citaService.ObtenerPorPaciente(pacienteId));
        }
    }
}