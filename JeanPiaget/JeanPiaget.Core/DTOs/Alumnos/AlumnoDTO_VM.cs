using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.Pagos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.DTOs.Alumnos
{
    public class AlumnoDTO_VM
    {
        public Alumno Alumno { get; set; }
        public Pago Pago { get; set; }
        public bool Status { get; set; }
    }
}
