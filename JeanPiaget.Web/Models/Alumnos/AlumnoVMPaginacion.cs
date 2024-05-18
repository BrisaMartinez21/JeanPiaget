using JeanPiaget.Core.Alumnos;

namespace JeanPiaget.Web.Models.Alumnos
{
    public class AlumnoVMPaginacion : BaseModeloPaginacion
    {
        public List<Alumno> Alumnos { get; set; }
    }
}
