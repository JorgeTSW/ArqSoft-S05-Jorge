using CitasApp.Application.Services;
using CitasApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Web.Controllers
{
    [Authorize]
    public class PacienteController : Controller
    {
        private readonly PacienteService _service;

        public PacienteController(PacienteService service)
        {
            _service = service;
        }

        public IActionResult Index() => View(_service.ObtenerTodos());

        public IActionResult Detalle(int id)
        {
            var paciente = _service.ObtenerPorId(id);
            return paciente == null ? NotFound() : View(paciente);
        }
    }
}