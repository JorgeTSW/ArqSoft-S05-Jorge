using CitasApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Web.Controllers
{
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
            ViewBag.Pacientes = _pacienteService.ObtenerTodos();
            ViewBag.Medicos = _medicoService.ObtenerTodos();
            return View(_citaService.ObtenerTodos());
        }

        public IActionResult PorPaciente(int pacienteId)
        {
            ViewBag.Pacientes = _pacienteService.ObtenerTodos();
            ViewBag.Medicos = _medicoService.ObtenerTodos();
            return View(_citaService.ObtenerPorPaciente(pacienteId));
        }
    }
}