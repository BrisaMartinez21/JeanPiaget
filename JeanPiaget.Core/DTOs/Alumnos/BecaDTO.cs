using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.DTOs.Alumnos
{
    public class BecaDTO
    {
        [Required]
        public int Descuento { get; set; }
    }
}
