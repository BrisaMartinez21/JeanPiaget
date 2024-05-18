using JeanPiaget.Core.Materias;
using JeanPiaget.Core.Pagos;
using JeanPiaget.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Materias
{
    public class GradoAppService : IGradoAppService
    {
        private readonly IRepository<int, Grado> _repository;
        public GradoAppService(IRepository<int, Grado> repository)
        {
            _repository = repository;
        }

        public async Task<List<Grado>> GetGradosAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<int> AddGradoAsync(Grado grado)
        {
            await _repository.AddAsync(grado);
            return grado.Id;
        }

        public async Task DeleteGradoAsync(int gradoId)
        {
            await _repository.DeleteAsync(gradoId);
        }

        public async Task<Grado> GetGradoAsync(int gradoId)
        {
            return await _repository.GetAsync(gradoId);
        }

        public async Task EditGradoAsync(Grado grado)
        {
            await _repository.UpdateAsync(grado);
        }

        public async Task<List<SelectListItem>> GetGradosNames()
        {
            return await _repository.GetAll()
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Fase + "° de " + d.Nivel,
                })
                .ToListAsync();
        }
    }
}
