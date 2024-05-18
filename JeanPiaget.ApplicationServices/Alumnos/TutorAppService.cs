using AutoMapper;
using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.DTOs.Alumnos;
using JeanPiaget.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Alumnos
{
    public class TutorAppService : ITutorAppService
    {
        private readonly IRepository<int, Tutor> _repository;
        public TutorAppService(IRepository<int, Tutor> repository)
        {
            _repository = repository;
        }

        public async Task<List<Tutor>> GetTutorsAsync()
        {
            return await _repository.GetAll()
                .ToListAsync();
        }

        public async Task<int> AddTutorAsync(Tutor tutor)
        {
            await _repository.AddAsync(tutor);
            return tutor.Id;
        }

        public async Task DeleteTutorAsync(int tutorId)
        {
            await _repository.DeleteAsync(tutorId);
        }

        public async Task<Tutor> GetTutorAsync(int tutorId)
        {
            return await _repository.GetAll()
                .Where(o => o.Id == tutorId)
                .FirstOrDefaultAsync();
        }

        public async Task EditTutorAsync(Tutor tutor)
        {
            await _repository.UpdateAsync(tutor);
        }
    }
}
