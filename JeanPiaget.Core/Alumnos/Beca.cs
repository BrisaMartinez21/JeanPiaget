using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JeanPiaget.Core.Alumnos
{
    public class Beca
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Descuento { get; set; }
    }
}
