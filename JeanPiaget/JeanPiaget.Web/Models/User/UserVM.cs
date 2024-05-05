using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JeanPiaget.Web.Models.User
{
    public class UserVM
    {
        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo Nombre es requerido!")]
        public string Nombre { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo de apellido Paterno es requerido!")]
        public string Paterno { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo de apellido Materno es requerido!")]
        public string Materno { get; set; }
        [Required(ErrorMessage = "¡Selecciona un género!")]
        public string Genero { get; set; }
        [Required(ErrorMessage = "¡El campo de Fecha de Nacimiento es requerido!")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaNacimiento { get; set; }
        [StringLength(30)]
        [Required(ErrorMessage = "¡El campo Cargo es requerido!")]
        public string Cargo { get; set; }
        [Required(ErrorMessage = "¡Selecciona un Rol!")]
        public string Rol { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^\d+$", ErrorMessage = "¡El campo de Teléfono solo debe contener números enteros!")]
        [Required(ErrorMessage = "¡El campo Télefono es requerido!")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "¡El campo Email es requerido!")]
        [EmailAddress(ErrorMessage = "¡El campo Email no tiene un formato de correo electrónico válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "¡El campo Contraseña es requerido!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "La contraseña debe contener al menos 8 caracteres, incluyendo al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "¡El campo Confirmar contraseña es requerido!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "La contraseña debe contener al menos 8 caracteres, incluyendo al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string ConfirmPassword { get; set; }
    }
}
