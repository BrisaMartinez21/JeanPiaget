using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.DTOs.Alumnos
{
    public class TutorListDTO
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Paterno { get; set; }
        [Required]
        public string Materno { get; set; }
        [Required, StringLength(10)]
        public string Telefono { get; set; }
        [Required, StringLength(50)]
        public string Correo { get; set; }
        [Required]
        public int AlumnoId { get; set; }
    }
}
