using JeanPiaget.Core.DTOs.Alumnos;
using JeanPiaget.Core.DTOs.Pagos;
using JeanPiaget.Core.Pagos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Pagos
{
    public interface IPagoAppService
    {
        Task<List<PagoListDTO>> GetPagosDTOAsync();
        Task<List<Pago>> GetPagosAsync();

        Task<int> AddPagoAsync(Pago pago);

        Task DeletePagoAsync(int pagoId);
        Task<Pago> GetPagoAsync(int pagoId);

        Task EditPagoAsync(Pago pago);

        Task<List<Pago>> GetPagosFilterAsync(string busqueda, int filtro, int pagina, int cantidad);

        Task<int> TotalPagosFilterAsync(string busqueda, int filtro);

        Task<List<Pago>> PaginationPagos(int pagina, int cantidad);

        Task<int> TotalPagos();

        Task<List<AlumnoDTO_VM>> GetPagosAlumnosAsync(List<AlumnoDTO_VM> alumnos);

        Task<bool> ExisteReferenciaAsync(string referencia);

        Task<List<Pago>> GetPagosCurrentYearAsync(int alumnoId);
    }
}
