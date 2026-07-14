using System.ComponentModel.DataAnnotations;

namespace CitasApp.Web.ViewModels
{
    public class IniciarSesionViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Por favor, introduce un correo válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme en este equipo")]
        public bool RememberMe { get; set; }
    }
}