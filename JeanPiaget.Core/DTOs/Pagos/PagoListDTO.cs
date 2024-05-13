using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.DTOs.Pagos
{
    public class PagoListDTO
    {
        public int Id { get; set; }
        [Required]
        public int Monto { get; set; }
        [Required]
        public string Folio { get; set; }
        [Required]
        public string Referencia { get; set; }
        [Required]
        public DateTime FechaPago { get; set; }
        [Required]
        public int Modalidad { get; set; }
        [Required]
        public int AlumnoId { get; set; }
    }
}
