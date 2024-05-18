using AutoMapper;
using JeanPiaget.Core.DTOs.Alumnos;
using JeanPiaget.Core.DTOs.Pagos;
using JeanPiaget.Core.Pagos;
using JeanPiaget.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Pagos
{
    public class PagoAppService : IPagoAppService
    {
        private readonly IRepository<int, Pago> _repository;
        private readonly IMapper _mapper;
        public PagoAppService(IRepository<int, Pago> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PagoListDTO>> GetPagosDTOAsync()
        {
            return _mapper.Map<List<PagoListDTO>>(
                await _repository.GetAll()
                .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                .Include(o => o.Alumno).ThenInclude(u => u.Tutor)
                .ToListAsync());
        }
        public async Task<List<Pago>> GetPagosAsync()
        {
            return await _repository.GetAll()
                .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                .Include(o => o.Alumno).ThenInclude(u => u.Tutor)
                .ToListAsync();
        }

        public async Task<int> AddPagoAsync(Pago pago)
        {
            await _repository.AddAsync(pago);
            return pago.Id;
        }

        public async Task DeletePagoAsync(int pagoId)
        {
            await _repository.DeleteAsync(pagoId);
        }

        public async Task<Pago> GetPagoAsync(int pagoId)
        {
            return await _repository.GetAll()
                .Where(o => o.Id == pagoId)
                .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                .Include(o => o.Alumno).ThenInclude(u => u.Tutor)
                .FirstOrDefaultAsync();
        }

        public async Task EditPagoAsync(Pago pago)
        {
            await _repository.UpdateAsync(pago);
        }
        public async Task<List<AlumnoDTO_VM>> GetPagosAlumnosAsync(List<AlumnoDTO_VM> alumnos)
        {
            var fechaActual = DateTime.Now;
            foreach (var alumno in alumnos) 
            {
                var pagos = await _repository.GetAll()
                .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                .Include(o => o.Alumno).ThenInclude(u => u.Tutor)
                .Where(x => x.Alumno.Id == alumno.Alumno.Id && x.FechaPago.Year == fechaActual.Year)
                .OrderBy(x => x.FechaPago)
                .ToListAsync();
                var deudorMesActual = true;
                var contPagos = 0;
                var ultimoPago = new Pago();
                var cuotaTecnologica = false;
                var cuotaAnual = false;
                foreach (var pago in pagos)
                {
                    if (pago.Concepto != "Cuota Tecnológica" && pago.Concepto != "Cuota Anual")
                    {
                        if (pago.Concepto.ToLower() == ObtenerNombreMes(fechaActual.Month).ToLower()) 
                            //significa que pago el mes actual
                        {
                            alumno.Pago = pago;
                            alumno.Status = true;
                            deudorMesActual = false;
                        }
                        contPagos++;
                        ultimoPago = pago;
                    }
                    else
                    { //Validar si tiene las otras dos cuotas
                        if (pago.Concepto == "Cuota Tecnológica")
                        {
                            cuotaTecnologica = true;
                        }
                        if (pago.Concepto == "Cuota Anual")
                        {
                            cuotaAnual = true;
                        }
                    }
                }

                if(contPagos >= alumno.Alumno.Modalidad) //tiene las mensualidades cubiertas
                {
                    alumno.Pago = ultimoPago;
                    alumno.Status = true;
                    deudorMesActual = false;
                }
                else if(contPagos < alumno.Alumno.Modalidad && contPagos == (fechaActual.Month - 1) && deudorMesActual) 
                    //Debe meses de mensualidad incluyendo el mes actual en el año pero va al corriente con los demás
                {
                    if(fechaActual.Day > 10) //Esta fuera del rango por lo tanto es deudor
                    {
                        alumno.Pago = ultimoPago;
                        alumno.Status = false;
                    }
                    else if(fechaActual.Day < 10) //Si esta dentro de los 10 días habiles de pago
                    {
                        alumno.Pago = ultimoPago;
                        alumno.Status = true;
                    }
                }
                else if (contPagos < alumno.Alumno.Modalidad && contPagos < (fechaActual.Month - 1) && deudorMesActual)
                    //Debe meses de mensualidad incluyendo el actual en el año y tiene meses retrasdos
                {
                    alumno.Pago = ultimoPago;
                    alumno.Status = false;
                }
                else if(contPagos < alumno.Alumno.Modalidad && contPagos < (fechaActual.Month - 1) && !deudorMesActual)
                //Debe meses de mensualidad sin incluir el actual en el año y tiene meses retrasdos
                {
                    alumno.Pago = ultimoPago;
                    alumno.Status = false;
                }

                if (fechaActual.Month == 12 && (!cuotaTecnologica || !cuotaAnual))
                { //Validar si es el ultimo mes del año y aun se deben cuotas
                    alumno.Pago = ultimoPago;
                    alumno.Status = false;
                }
            }
            return alumnos;
        }

        public async Task<bool> ExisteReferenciaAsync(string referencia)
        {
            var lista = await _repository.GetAll()
                .Where(x => x.Referencia == referencia).ToListAsync();
            return lista != null && lista.Count > 0;
        }

        public async Task<List<Pago>> GetPagosCurrentYearAsync(int alumnoId)
        {
            return await _repository.GetAll()
                .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                .Include(o => o.Alumno).ThenInclude(u => u.Tutor)
                .Where(x => x.Alumno.Id == alumnoId && DateTime.Now.Year == x.FechaPago.Year)
                .ToListAsync();
        }

        public async Task<List<Pago>> GetPagosFilterAsync(string busqueda, int filtro, int pagina, int cantidad)
        {
            switch (filtro)
            {
                case 1:
                    return await _repository.GetAll()
                        .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                        .Include(o => o.Alumno).ThenInclude(u => u.Tutor)
                        .Where(x => (x.Alumno.Nombre.ToLower() + " " + x.Alumno.Paterno.ToLower() + " " + x.Alumno.Materno.ToLower())
                        .Contains(busqueda.ToLower()))
                        .OrderByDescending(x => x.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
                case 2:
                    return await _repository.GetAll()
                        .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                        .Include(o => o.Alumno).ThenInclude(u => u.Tutor)
                        .Where(x => (x.Alumno.Grado.Nivel.ToLower() + " " + x.Alumno.Grado.Fase)
                        .Contains(busqueda.ToLower()))
                        .OrderByDescending(x => x.Id)
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToListAsync();
                    break;
            }
            return await Task.FromResult(new List<Pago>());
        }

        public async Task<int> TotalPagosFilterAsync(string busqueda, int filtro)
        {
            switch (filtro)
            {
                case 1:
                    return await _repository.GetAll()
                        .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                        .Where(x => (x.Alumno.Nombre.ToLower() + " " + x.Alumno.Paterno.ToLower() + " " + x.Alumno.Materno.ToLower())
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
                case 2:
                    return await _repository.GetAll()
                        .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                        .Where(x => (x.Alumno.Grado.Nivel.ToLower() + " " + x.Alumno.Grado.Fase)
                        .Contains(busqueda.ToLower())).CountAsync();
                    break;
            }
            return 0;
        }

        public async Task<List<Pago>> PaginationPagos(int pagina, int cantidad)
        {
            return await _repository.GetAll()
                .Include(o => o.Alumno).ThenInclude(u => u.Grado)
                .Include(o => o.Alumno).ThenInclude(u => u.Tutor)
                .OrderByDescending(x => x.Id)
                .Skip((pagina - 1) * cantidad)
                .Take(cantidad).ToListAsync();
        }

        public async Task<int> TotalPagos()
        {
            return await _repository.GetAll().CountAsync();
        }

        public string ObtenerNombreMes(int numeroMes)
        {
            switch (numeroMes)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";
                default:
                    return "Cuota";
            }
        }
    }
}
