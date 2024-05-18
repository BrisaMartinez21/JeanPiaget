using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.DTOs.Usuarios
{
    public class UsuarioAddDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Paterno { get; set; }
        [Required]
        public string Materno { get; set; }
        [Required]
        public DateOnly FechaNacimiento { get; set; }
        [Required]
        public string Cargo { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string RoleId { get; set; }
    }
}
