using Microsoft.AspNetCore.Mvc.Rendering;

namespace JeanPiaget.Web.Models
{
    public class BaseModeloPaginacion
    {
        public int PaginaActual { get; set; }
        public int TotalDeRegistros { get; set; }
        public int RegistrosPorPagina { get; set; }
        public string BuscarItem { get; set; }
        public int FiltroBusqueda { get; set; }
        public List<SelectListItem> FiltrosDisponibles { get; set; }
        public bool EsBuscador { get; set; }
    }
}
