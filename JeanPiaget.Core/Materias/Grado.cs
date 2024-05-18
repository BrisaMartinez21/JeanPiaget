using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.Materias
{
    public class Grado
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nivel { get; set; }
        [Required]
        public int Fase { get; set; }
    }
}
