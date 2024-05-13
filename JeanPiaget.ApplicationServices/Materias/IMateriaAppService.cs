using JeanPiaget.Core.DTOs.Materias;
using JeanPiaget.Core.Materias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Materias
{
    public interface IMateriaAppService
    {
        Task<List<Materia>> GetMateriasAsync();

        Task<int> AddMateriaAsync(Materia materia);

        Task DeleteMateriaAsync(int materiaId);
        Task<Materia> GetMateriaAsync(int materiaId);

        Task EditMateriaAsync(Materia materia);

        Task<List<Materia>> GetMateriasFilterAsync(string busqueda, int filtro, int pagina, int cantidad);

        Task<int> TotalMateriasFilterAsync(string busqueda, int filtro);

        Task<List<Materia>> PaginationMaterias(int pagina, int cantidad);

        Task<int> TotalMaterias();

        Task<bool> ExisteClaveAsync(string clave);

        Task<List<Materia>> GetMateriasByGradoAsync(int gradoId);
    }
}
