using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.Usuarios
{
    public class Usuario : IdentityUser
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Paterno { get; set; }
        [Required]
        public string Materno { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required]
        public DateOnly FechaNacimiento { get; set; }
        [Required]
        public string Cargo { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
