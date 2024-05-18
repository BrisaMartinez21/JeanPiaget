using JeanPiaget.Core.Materias;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Materias
{
    public interface IGradoAppService
    {
        Task<List<Grado>> GetGradosAsync();

        Task<int> AddGradoAsync(Grado grado);

        Task DeleteGradoAsync(int gradoId);
        Task<Grado> GetGradoAsync(int gradoId);

        Task EditGradoAsync(Grado grado);
        Task<List<SelectListItem>> GetGradosNames();
    }
}
