using AutoMapper;
using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.DTOs.Alumnos;
using JeanPiaget.Core.DTOs.Materias;
using JeanPiaget.Core.Materias;
using JeanPiaget.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Alumnos
{
    public class AlumnoAppService : IAlumnoAppService
    {
        private readonly IRepository<int, Alumno> _repository;
        public AlumnoAppService(IRepository<int, Alumno> repository)
        {
            _repository = repository;
        }

        public async Task<List<Alumno>> GetAlumnosAsync()
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .Include(o => o.Tutor)
                .ToListAsync();
        }

        public async Task<int> AddAlumnoAsync(Alumno alumno)
        {
            await _repository.AddAsync(alumno);
            return alumno.Id;
        }

        public async Task DeleteAlumnoAsync(int alumnoId)
        {
            await _repository.DeleteAsync(alumnoId);
        }

        public async Task<Alumno> GetAlumnoAsync(int alumnoId)
        {
            return await _repository.GetAll()
                .Where(o => o.Id == alumnoId)
                .Include(o => o.Grado)
                .Include(o => o.Tutor)
                .FirstOrDefaultAsync();
        }

        public async Task EditAlumnoAsync(Alumno alumno)
        {
            await _repository.UpdateAsync(alumno);
        }

        public async Task<List<Alumno>> GetAlumnosAsync(string busqueda)
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .Include(o => o.Tutor)
                .Where(x => (x.Nombre.ToLower() + " " + x.Paterno.ToLower() + " " + x.Materno.ToLower())
                .Contains(busqueda.ToLower()))
                .OrderByDescending(x => x.Id).ToListAsync();
        }        
        public async Task<List<Alumno>> GetAlumnosAsync(string busqueda, int pagina, int cantidad)
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .Include(o => o.Tutor)
                .Where(x => (x.Nombre.ToLower() + " " + x.Paterno.ToLower() + " " + x.Materno.ToLower())
                .Contains(busqueda.ToLower()))
                .OrderByDescending(x => x.Id)
                .Skip((pagina - 1) * cantidad)
                .Take(cantidad).ToListAsync();
        }

        public async Task<List<Alumno>> GetAlumnosAsync(int gradoId)
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .Include(o => o.Tutor)
                .Where(x => x.Grado.Id == gradoId)
                .ToListAsync();
        }
        public async Task<int> GetTotalAlumnosGradoAsync(int gradoId)
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .Where(x => x.Grado.Id == gradoId)
                .CountAsync();
        }
        public async Task<bool> ExisteMatriculaAsync(string matricula)
        {
            var lista = await _repository.GetAll()
                .Where(x => x.Matricula == matricula).ToListAsync();
            return lista != null && lista.Count > 0;
        }

        public async Task<List<Alumno>> GetAlumnosFilterAsync(string busqueda, int filtro, int pagina, int cantidad)
        {
            switch (filtro)
            {
                case 1:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Include(o => o.Tutor)
                        .Where(x => (x.Nombre.ToLower() + " " + x.Paterno.ToLower() + " " + x.Materno.ToLower())
                        .Contains(busqueda.ToLower()))
                        .OrderBy(x => x.Grado.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
                case 2:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Include(o => o.Tutor)
                        .Where(x => x.Matricula
                        .Contains(busqueda.ToLower()))
                        .OrderBy(x => x.Grado.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
                case 3:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Include(o => o.Tutor)
                        .Where(x => (x.Grado.Nivel.ToLower() + " " + x.Grado.Fase)
                        .Contains(busqueda.ToLower()))
                        .OrderBy(x => x.Grado.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
                case 4:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Include(o => o.Tutor)
                        .Where(x => (x.Tutor.Nombre.ToLower() + " " + x.Tutor.Paterno.ToLower() + " " + x.Tutor.Materno.ToLower())
                        .Contains(busqueda.ToLower()))
                        .OrderBy(x => x.Grado.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
                case 5:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Include(o => o.Tutor)
                        .Where(x => x.Tutor.Telefono
                        .Contains(busqueda.ToLower()))
                        .OrderBy(x => x.Grado.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
            }
            return await Task.FromResult(new List<Alumno>());
        }

        public async Task<int> TotaAlumnosFilterAsync(string busqueda, int filtro)
        {
            switch (filtro)
            {
                case 1:
                    return await _repository.GetAll()
                        .Where(x => (x.Nombre.ToLower() + " " + x.Paterno.ToLower() + " " + x.Materno.ToLower())
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
                case 2:
                    return await _repository.GetAll()
                        .Where(x => x.Matricula
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
                case 3:
                    return await _repository.GetAll()
                        .Where(x => (x.Grado.Nivel.ToLower() + " " + x.Grado.Fase)
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
                case 4:
                    return await _repository.GetAll()
                        .Where(x => (x.Tutor.Nombre.ToLower() + " " + x.Tutor.Paterno.ToLower() + " " + x.Tutor.Materno.ToLower())
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
                case 5:
                    return await _repository.GetAll()
                        .Where(x => x.Tutor.Telefono
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
            }
            return 0;
        }
        public async Task<List<Alumno>> PaginationAlumnos(int pagina, int cantidad)
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .Include(o => o.Tutor)
                .OrderBy(x => x.Grado.Id)
                .Skip((pagina - 1) * cantidad)
                .Take(cantidad).ToListAsync();
        }

        public async Task<int> TotalAlumnos()
        {
            return await _repository.GetAll().CountAsync();
        }
    }
}
