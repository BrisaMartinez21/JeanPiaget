using AutoMapper;
using JeanPiaget.Core.Calificaciones;
using JeanPiaget.Core.DTOs.Calificaciones;
using JeanPiaget.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Calificaciones
{
    public class CalificacionAppService : ICalificacionAppService
    {
        private readonly IRepository<int, Calificacion> _repository;
        private readonly IMapper _mapper;
        public CalificacionAppService(IRepository<int, Calificacion> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CalificacionListDTO>> GetCalificacionsAsync()
        {
            return _mapper.Map<List<CalificacionListDTO>>(
                await _repository.GetAll()
                .Include(o => o.Alumno)
                .Include(o => o.Materia)
                .ToListAsync());
        }

        public async Task<int> AddCalificacionAsync(Calificacion calificacion)
        {
            await _repository.AddAsync(calificacion);
            return calificacion.Id;
        }

        public async Task DeleteCalificacionAsync(int calificacionId)
        {
            await _repository.DeleteAsync(calificacionId);
        }

        public async Task<CalificacionDTO> GetCalificacionDTOAsync(int calificacionId)
        {
            var calificacion = await _repository.GetAll()
                    .Where(o => o.Id == calificacionId)
                    .Include(o => o.Materia)
                    .Include(o => o.Alumno)
                    .FirstOrDefaultAsync();
            if (calificacion != null)
            {
                return _mapper.Map<CalificacionDTO>(calificacion);
            }
            else
            {
                return null;
            }
        }
        public async Task<Calificacion> GetCalificacionAsync(int calificacionId)
        {
            return await _repository.GetAll()
                    .Where(o => o.Id == calificacionId)
                    .Include(o => o.Materia)
                    .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                    .FirstOrDefaultAsync();
        }

        public async Task<CalificacionListDTO> GetCalificacionGetDTOAsync(int calificacionId)
        {
            var calificacion = await _repository.GetAll()
                    .Where(o => o.Id == calificacionId)
                    .Include(o => o.Materia)
                    .Include(o => o.Alumno)
                    .FirstOrDefaultAsync();
            if (calificacion != null)
            {
                return _mapper.Map<CalificacionListDTO>(calificacion);
            }
            else
            {
                return null;
            }
        }

        public async Task EditCalificacionAsync(Calificacion calificacion)
        {
            await _repository.UpdateAsync(calificacion);
        }
    }
}
