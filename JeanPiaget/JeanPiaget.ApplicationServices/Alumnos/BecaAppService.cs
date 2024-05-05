using JeanPiaget.Core.Alumnos;
using JeanPiaget.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Alumnos
{
    public class BecaAppService : IBecaAppService
    {
        private readonly IRepository<int, Beca> _repository;
        public BecaAppService(IRepository<int, Beca> repository)
        {
            _repository = repository;
        }

        public async Task<List<Beca>> GetBecasAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<int> AddBecaAsync(Beca beca)
        {
            await _repository.AddAsync(beca);
            return beca.Id;
        }

        public async Task DeleteBecaAsync(int becaId)
        {
            await _repository.DeleteAsync(becaId);
        }

        public async Task<Beca> GetBecaAsync(int becaId)
        {
            return await _repository.GetAsync(becaId);
        }

        public async Task EditBecaAsync(Beca beca)
        {
            await _repository.UpdateAsync(beca);
        }


        public async Task<List<SelectListItem>> GetBecasNames()
        {
            return await _repository.GetAll()
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Descuento+"%",
                })
                .ToListAsync();
        }
    }
}
