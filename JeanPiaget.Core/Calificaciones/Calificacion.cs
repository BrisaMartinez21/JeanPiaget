using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.Materias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.Calificaciones
{
    public class Calificacion
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public int Clave { get; set; }
        [Required]
        public int Nota { get; set; }
        [Required]
        public Materia Materia { get; set; }
        [Required]
        public Alumno Alumno { get; set; }
    }
}
