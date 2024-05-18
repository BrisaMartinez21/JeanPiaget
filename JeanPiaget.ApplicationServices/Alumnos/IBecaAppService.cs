using JeanPiaget.Core.Alumnos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Alumnos
{
    public interface IBecaAppService
    {
        Task<List<Beca>> GetBecasAsync();

        Task<int> AddBecaAsync(Beca beca);

        Task DeleteBecaAsync(int becaId);
        Task<Beca> GetBecaAsync(int becaId);

        Task EditBecaAsync(Beca beca);
        Task<List<SelectListItem>> GetBecasNames();
    }
}
