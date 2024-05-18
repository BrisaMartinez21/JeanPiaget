using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.DTOs.Alumnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Alumnos
{
    public interface IAlumnoAppService
    {
        Task<List<Alumno>> GetAlumnosAsync();
        Task<List<Alumno>> GetAlumnosAsync(string filter);
        Task<List<Alumno>> GetAlumnosAsync(string busqueda, int pagina, int cantidad);
        Task<List<Alumno>> GetAlumnosAsync(int gradoId);
        Task<int> GetTotalAlumnosGradoAsync(int gradoId);
        Task<int> AddAlumnoAsync(Alumno alumno);
        Task DeleteAlumnoAsync(int alumnoId);
        Task<Alumno> GetAlumnoAsync(int alumnoId);
        Task EditAlumnoAsync(Alumno alumno);
        Task<bool> ExisteMatriculaAsync(string matricula);
        Task<List<Alumno>> GetAlumnosFilterAsync(string busqueda, int filtro, int pagina, int cantidad);
        Task<int> TotaAlumnosFilterAsync(string busqueda, int filtro);
        Task<List<Alumno>> PaginationAlumnos(int pagina, int cantidad);
        Task<int> TotalAlumnos();
    }
}
