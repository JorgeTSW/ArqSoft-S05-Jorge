using CitasApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Web.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CuentaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ==========================================
        // REGISTRO
        // ==========================================

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var usuario = new IdentityUser { UserName = modelo.Email, Email = modelo.Email };
                var resultado = await _userManager.CreateAsync(usuario, modelo.Password);

                if (resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(modelo);
        }

        // ==========================================
        // INICIO DE SESIÓN (LOGIN)
        // ==========================================

        // GET: /Cuenta/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Cuenta/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(IniciarSesionViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                // El tercer parámetro indica si la sesión debe persistir (RememberMe)
                // El cuarto parámetro (false) indica si se bloquea la cuenta tras varios intentos fallidos
                var resultado = await _signInManager.PasswordSignInAsync(
                    modelo.Email,
                    modelo.Password,
                    modelo.RememberMe,
                    lockoutOnFailure: false);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Error genérico para no dar pistas a posibles atacantes sobre qué falló (si el correo o la clave)
                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
            }

            return View(modelo);
        }

        // POST: /Cuenta/Salir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Salir()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}