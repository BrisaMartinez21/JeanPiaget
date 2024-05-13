using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.Pagos;

namespace JeanPiaget.Web.Models.Pagos
{
    public class PagoConsultarVM
    {
        public List<Pago> Pagos { get; set; }
        public Alumno Alumno { get; set; }
    }
}
