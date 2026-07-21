using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculadoraController : ControllerBase
    {
        // GET /api/calculadora/sumar?a=5&b=3
        [HttpGet("sumar")]
        public IActionResult Sumar(double a, double b)
            => Ok(new { operacion = "suma", a, b, resultado = a + b });

        // GET /api/calculadora/restar?a=5&b=3
        [HttpGet("restar")]
        public IActionResult Restar(double a, double b)
            => Ok(new { operacion = "resta", a, b, resultado = a - b });

        // GET /api/calculadora/multiplicar?a=5&b=3
        [HttpGet("multiplicar")]
        public IActionResult Multiplicar(double a, double b)
            => Ok(new { operacion = "multiplicacion", a, b, resultado = a * b });

        // GET /api/calculadora/dividir?a=10&b=2
        [HttpGet("dividir")]
        public IActionResult Dividir(double a, double b)
        {
            if (b == 0)
                return BadRequest(new { error = "No se puede dividir entre cero" });

            return Ok(new { operacion = "division", a, b, resultado = a / b });
        }
    }
}