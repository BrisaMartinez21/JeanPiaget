using AutoMapper;
using JeanPiaget.Core.DTOs.Materias;
using JeanPiaget.Core.Materias;
using JeanPiaget.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Materias
{
    public class MateriaAppService : IMateriaAppService
    {
        private readonly IRepository<int, Materia> _repository;
        public MateriaAppService(IRepository<int, Materia> repository)
        {
            _repository = repository;
        }

        public async Task<List<Materia>> GetMateriasAsync()
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .ToListAsync();
        }

        public async Task<int> AddMateriaAsync(Materia materia)
        {
            await _repository.AddAsync(materia);
            return materia.Id;
        }

        public async Task DeleteMateriaAsync(int materiaId)
        {
            await _repository.DeleteAsync(materiaId);
        }

        public async Task<Materia> GetMateriaAsync(int materiaId)
        {
            return await _repository.GetAll()
                .Where(materia => materia.Id == materiaId)
                .Include(o => o.Grado)
                .FirstOrDefaultAsync();
        }

        public async Task EditMateriaAsync(Materia materia)
        {
            await _repository.UpdateAsync(materia);
        }
        public async Task<bool> ExisteClaveAsync(string clave)
        {
            var lista = await _repository.GetAll()
                .Where(x => x.Clave.ToLower() == clave.ToLower()).ToListAsync();
            return lista != null && lista.Count > 0;
        }
        public async Task<List<Materia>> GetMateriasByGradoAsync(int gradoId)
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .Where(x => x.Grado.Id == gradoId)
                .ToListAsync();
        }

        public async Task<List<Materia>> GetMateriasFilterAsync(string busqueda, int filtro, int pagina, int cantidad)
        {
            switch (filtro)
            {
                case 1:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Where(x => x.Nombre.ToLower()
                        .Contains(busqueda.ToLower()))
                        .OrderBy(x => x.Grado.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
                case 2:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Where(x => x.Clave.ToLower()
                        .Contains(busqueda.ToLower()))
                        .OrderBy(x => x.Grado.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
                case 3:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Where(x => (x.Grado.Nivel.ToLower() + " " + x.Grado.Fase)
                        .Contains(busqueda.ToLower()))
                        .OrderBy(x => x.Grado.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
            }
            return await Task.FromResult(new List<Materia>());
        }

        public async Task<int> TotalMateriasFilterAsync(string busqueda, int filtro)
        {
            switch (filtro)
            {
                case 1:
                    return await _repository.GetAll()
                        .Where(x => x.Nombre.ToLower()
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
                case 2:
                    return await _repository.GetAll()
                        .Where(x => x.Clave.ToLower()
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
                case 3:
                    return await _repository.GetAll()
                        .Include(o => o.Grado)
                        .Where(x => (x.Grado.Nivel.ToLower() + " " + x.Grado.Fase)
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
            }
            return 0;
        }

        public async Task<List<Materia>> PaginationMaterias(int pagina, int cantidad)
        {
            return await _repository.GetAll()
                .Include(o => o.Grado)
                .OrderBy(x => x.Grado.Id)
                .Skip((pagina - 1) * cantidad)
                .Take(cantidad).ToListAsync();
        }

        public async Task<int> TotalMaterias()
        {
            return await _repository.GetAll().CountAsync();
        }
    }
}
