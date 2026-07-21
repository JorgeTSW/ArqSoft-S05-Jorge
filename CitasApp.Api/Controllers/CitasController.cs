using CitasApp.Application.Services;
using CitasApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly CitaService _citaService;
        private readonly PacienteService _pacienteService;
        private readonly MedicoService _medicoService;

        public CitasController(CitaService citaService,
                               PacienteService pacienteService,
                               MedicoService medicoService)
        {
            _citaService = citaService;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_citaService.ObtenerTodos());

        [HttpGet("porpaciente/{pacienteId}")]
        public IActionResult PorPaciente(int pacienteId)
        {
            var citas = _citaService.ObtenerPorPaciente(pacienteId);
            return citas.Count == 0 ? NotFound() : Ok(citas);
        }

        [HttpPost("confirmar/{citaId}")]
        public IActionResult ConfirmarCita(int citaId)
        {
            var citas = _citaService.ObtenerTodos();
            var cita = citas.FirstOrDefault(c => c.Id == citaId);

            if (cita == null) return NotFound();

            _citaService.ConfirmarCita(cita);
            return Ok(new { mensaje = "Cita confirmada", cita });
        }
        /*
        [HttpGet("confirmar/{citaId}")]
        public IActionResult ConfirmarCita(int citaId)
        {
            var citas = _citaService.ObtenerTodos();
            var cita = citas.FirstOrDefault(c => c.Id == citaId);

            if (cita == null) return NotFound();

            _citaService.ConfirmarCita(cita);
            return Ok(new { mensaje = "Cita confirmada", cita });
        }
        */
    }
}