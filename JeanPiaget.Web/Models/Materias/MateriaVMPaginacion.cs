using JeanPiaget.Core.Materias;

namespace JeanPiaget.Web.Models.Materias
{
    public class MateriaVMPaginacion : BaseModeloPaginacion
    {
        public List<Materia> Materias { get; set; }
    }
}
