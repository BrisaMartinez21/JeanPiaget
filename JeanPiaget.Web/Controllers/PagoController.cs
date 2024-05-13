using JeanPiaget.ApplicationServices.Alumnos;
using JeanPiaget.ApplicationServices.Materias;
using JeanPiaget.ApplicationServices.Pagos;
using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.DTOs.Alumnos;
using JeanPiaget.Core.Pagos;
using JeanPiaget.Web.Models.Alumnos;
using JeanPiaget.Web.Models.Pagos;
using Microsoft.AspNetCore.Mvc;

namespace JeanPiaget.Web.Controllers
{
    public class PagoController : Controller
    {
        private readonly IPagoAppService _pagoAppService;
        private readonly IAlumnoAppService _alumnoAppService;
        private readonly IGradoAppService _gradoAppService;

        public PagoController(IPagoAppService pagoAppService, IAlumnoAppService alumnoAppService, IGradoAppService gradoAppService)
        {
            _pagoAppService = pagoAppService;
            _alumnoAppService = alumnoAppService;
            _gradoAppService = gradoAppService;
        }
        public async Task<ActionResult> Index(int? pagina, string? buscarItem, int? filtroBusqueda)
        {
            int cantidadRegistros = 25;
            PagoVMPaginacion modelo = new PagoVMPaginacion();
            if (!string.IsNullOrEmpty(buscarItem) && filtroBusqueda.HasValue)
            {
                if (pagina.HasValue)
                {
                    var alumnos = await _alumnoAppService.GetAlumnosFilterAsync(buscarItem, filtroBusqueda.Value, pagina.Value, cantidadRegistros);
                    List<AlumnoDTO_VM> newAlumnos = new List<AlumnoDTO_VM>();
                    foreach (var alumno in alumnos)
                    {
                        newAlumnos.Add(new AlumnoDTO_VM { Alumno = alumno });
                    }
                    List<AlumnoDTO_VM> listAlumnos = await _pagoAppService.GetPagosAlumnosAsync(newAlumnos);
                    modelo.Alumnos = listAlumnos;
                    modelo.PaginaActual = pagina.Value;
                    modelo.TotalDeRegistros = await _alumnoAppService.TotaAlumnosFilterAsync(buscarItem, filtroBusqueda.Value);
                    modelo.BuscarItem = buscarItem;
                    modelo.FiltroBusqueda = filtroBusqueda.Value;
                    modelo.EsBuscador = true;
                }
                else
                {
                    var alumnos = await _alumnoAppService.GetAlumnosFilterAsync(buscarItem, filtroBusqueda.Value, 1, cantidadRegistros);
                    List<AlumnoDTO_VM> newAlumnos = new List<AlumnoDTO_VM>();
                    foreach (var alumno in alumnos)
                    {
                        newAlumnos.Add(new AlumnoDTO_VM { Alumno = alumno });
                    }
                    List<AlumnoDTO_VM> listAlumnos = await _pagoAppService.GetPagosAlumnosAsync(newAlumnos);
                    modelo.Alumnos = listAlumnos;
                    modelo.PaginaActual = 1;
                    modelo.TotalDeRegistros = await _alumnoAppService.TotaAlumnosFilterAsync(buscarItem, filtroBusqueda.Value);
                    modelo.BuscarItem = buscarItem;
                    modelo.FiltroBusqueda = filtroBusqueda.Value;
                    modelo.EsBuscador = true;
                }
            }
            else
            {
                if (!pagina.HasValue)
                {
                    var alumnos = await _alumnoAppService.PaginationAlumnos(1, cantidadRegistros);
                    List<AlumnoDTO_VM> newAlumnos = new List<AlumnoDTO_VM>();
                    foreach (var alumno in alumnos)
                    {
                        newAlumnos.Add(new AlumnoDTO_VM { Alumno = alumno });
                    }
                    List<AlumnoDTO_VM> listAlumnos = await _pagoAppService.GetPagosAlumnosAsync(newAlumnos);
                    modelo.Alumnos = listAlumnos;
                    modelo.PaginaActual = 1;
                }
                else
                {
                    var alumnos = await _alumnoAppService.PaginationAlumnos(pagina.Value, cantidadRegistros); List<AlumnoDTO_VM> newAlumnos = new List<AlumnoDTO_VM>();
                    foreach (var alumno in alumnos)
                    {
                        newAlumnos.Add(new AlumnoDTO_VM { Alumno = alumno });
                    }
                    List<AlumnoDTO_VM> listAlumnos = await _pagoAppService.GetPagosAlumnosAsync(newAlumnos);
                    modelo.Alumnos = listAlumnos;
                    modelo.PaginaActual = pagina.Value;
                }
                modelo.TotalDeRegistros = await _alumnoAppService.TotalAlumnos();
                modelo.EsBuscador = false;
            }
            modelo.RegistrosPorPagina = cantidadRegistros;

            return View(modelo);
        }

        public async Task<ActionResult> Create(int id)
        {
            var fechaActual = DateTime.Now;
            var alumno = await _alumnoAppService.GetAlumnoAsync(id);
            PagoVM model = new PagoVM()
            {
                Id = id,
                Nombre = alumno.Nombre + " " + alumno.Paterno + " "+ alumno.Materno,
                Nivel = alumno.Grado.Nivel,
                Fase = alumno.Grado.Fase,
                Beca = alumno.Beca != null ? alumno.Beca.Value : 0,
                Mensualidad = alumno.Mensualidad,
                TotalPagar = alumno.Beca != null ? (float)Math.Round(alumno.Mensualidad - ((float)alumno.Beca.Value / 100 * alumno.Mensualidad), 2) : alumno.Mensualidad,
                FechaActual = new DateOnly(fechaActual.Year, fechaActual.Month, fechaActual.Day),
                Grados = await _gradoAppService.GetGradosNames(),
                GradoId = alumno.Grado.Id,
            };
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Create(PagoVM model)
        {
            var fechaActual = DateTime.Now;
            try
            {
                if (ModelState.IsValid && model.Concepto != 0 && model.TotalPagar==model.Monto)
                {
                    var alumno = await _alumnoAppService.GetAlumnoAsync(model.Id);
                    var pago = new Pago()
                    {
                        Monto = model.Monto,
                        Folio = model.Folio,
                        Concepto = ObtenerNombreMes(model.Concepto),
                        FechaPago = DateTime.Now,
                        Alumno = alumno,
                    };

                    Random random = new Random();
                    bool salida = true;
                    int digitosYear = DateTime.Now.Year % 100;
                    while (salida)
                    {
                        int parteReferencia = random.Next(11110, 99999);
                        if (model.Concepto < 10)
                        {
                            pago.Referencia = "1602" + digitosYear + "0" + model.Concepto + parteReferencia;
                        }
                        else
                        {
                            pago.Referencia = "1602" + digitosYear + model.Concepto + parteReferencia;
                        }
                        if (!await _pagoAppService.ExisteReferenciaAsync(pago.Referencia))
                        {
                            salida = false; break;
                        }
                    }

                    await _pagoAppService.AddPagoAsync(pago);

                    if(model.GradoId != null && model.GradoId != alumno.Grado.Id) 
                    {
                        alumno.Grado = await _gradoAppService.GetGradoAsync(model.GradoId.Value);
                        await _alumnoAppService.EditAlumnoAsync(alumno);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    model.FechaActual = new DateOnly(fechaActual.Year, fechaActual.Month, fechaActual.Day);
                    if (model.Concepto == 0)
                    {
                        ModelState.AddModelError("Concepto", "¡Selecciona un Concepto!");
                    }

                    if (model.TotalPagar < model.Monto)
                    {
                        ModelState.AddModelError("Monto", "¡El Monto es menor al Total a Pagar!");
                    }
                    else if (model.TotalPagar > model.Monto)
                    {
                        ModelState.AddModelError("Monto", "¡El Monto es mayor al Total a Pagar!");
                    }
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                model.FechaActual = new DateOnly(fechaActual.Year, fechaActual.Month, fechaActual.Day);
                return View(model);
            }
        }

        public async Task<ActionResult> Consultar(int id)
        {
            PagoConsultarVM model = new PagoConsultarVM()
            {
                Pagos = await _pagoAppService.GetPagosCurrentYearAsync(id),
                Alumno = await _alumnoAppService.GetAlumnoAsync(id),
            };
            return View(model);
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
                case 13:
                    return "Cuota Tecnológica";
                case 14:
                    return "Cuota Anual";
                default:
                    return "Null";
            }
        }
    }
}
