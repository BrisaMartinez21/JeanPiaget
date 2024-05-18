using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.DTOs.Materias
{
    public class GradoDTO
    {
        [Required]
        public string Nivel { get; set; }
        [Required]
        public int Fase { get; set; }
    }
}
