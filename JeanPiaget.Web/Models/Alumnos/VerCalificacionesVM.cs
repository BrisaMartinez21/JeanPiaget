using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.Materias;

namespace JeanPiaget.Web.Models.Alumnos
{
    public class VerCalificacionesVM
    {
        public List<Materia> Materias { get; set; }

        public Alumno Alumno { get; set; }
    }
}
