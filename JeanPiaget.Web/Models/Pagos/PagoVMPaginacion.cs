using JeanPiaget.Core.DTOs.Alumnos;
using JeanPiaget.Core.Pagos;

namespace JeanPiaget.Web.Models.Pagos
{
    public class PagoVMPaginacion : BaseModeloPaginacion
    {
        public List<AlumnoDTO_VM> Alumnos { get; set; }
    }
}
