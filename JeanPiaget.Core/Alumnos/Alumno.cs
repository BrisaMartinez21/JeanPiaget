using JeanPiaget.Core.Materias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.Alumnos
{
    public class Alumno
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
        public DateOnly FechaNacimiento { get; set; }
        [Required] 
        public string Matricula { get; set; }
        [Required] 
        public float Mensualidad { get; set; }
        [Required]
        public int Modalidad { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required] 
        public Grado Grado { get; set; }
        [Required]
        public Tutor Tutor { get; set; }
        public int? Beca { get; set; }
    }
}
