using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.Alumnos
{
    public class Tutor
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Paterno { get; set; }
        [Required]
        public string Materno { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required, StringLength(10)]
        public string Telefono { get; set; }
        [Required, StringLength(50)]
        public string Correo { get; set; }
    }
}
