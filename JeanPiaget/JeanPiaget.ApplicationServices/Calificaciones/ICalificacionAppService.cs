using JeanPiaget.Core.Calificaciones;
using JeanPiaget.Core.DTOs.Calificaciones;
using JeanPiaget.Core.Materias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Calificaciones
{
    public interface ICalificacionAppService
    {
        Task<List<CalificacionListDTO>> GetCalificacionsAsync();

        Task<int> AddCalificacionAsync(Calificacion calificacion);

        Task DeleteCalificacionAsync(int calificacionId);
        Task<CalificacionDTO> GetCalificacionDTOAsync(int calificacionId);
        Task<CalificacionListDTO> GetCalificacionGetDTOAsync(int calificacionId);
        Task<Calificacion> GetCalificacionAsync(int calificacionId);

        Task EditCalificacionAsync(Calificacion calificacion);
    }
}
