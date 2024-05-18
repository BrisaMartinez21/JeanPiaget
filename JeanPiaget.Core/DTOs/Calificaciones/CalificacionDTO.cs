using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.DTOs.Calificaciones
{
    public class CalificacionDTO
    {
        [Required]
        public int Clave { get; set; }
        [Required]
        public int Nota { get; set; }
        [Required]
        public int MateriaId { get; set; }
        [Required]
        public int AlumnoId { get; set; }
    }
}
