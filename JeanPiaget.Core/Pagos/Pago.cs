using JeanPiaget.Core.Alumnos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.Pagos
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public float Monto { get; set; }
        [Required] 
        public string Folio { get; set; }
        [Required] 
        public string Referencia { get; set; }
        [Required]
        public string Concepto { get; set; }
        [Required]
        public DateTime FechaPago { get; set; }
        [Required]
        public Alumno Alumno { get; set; }
    }
}
