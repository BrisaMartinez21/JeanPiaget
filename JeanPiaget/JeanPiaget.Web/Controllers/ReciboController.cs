using JeanPiaget.ApplicationServices.Alumnos;
using JeanPiaget.ApplicationServices.Materias;
using JeanPiaget.ApplicationServices.Pagos;
using JeanPiaget.Core.Materias;
using JeanPiaget.Core.Pagos;
using JeanPiaget.DataAccess.Formatos;
using JeanPiaget.Web.Models.Recibo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Reporting.NETCore;
using Rotativa.AspNetCore.Options;
using Rotativa.AspNetCore;

namespace JeanPiaget.Web.Controllers
{
    public class ReciboController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IPagoAppService _pagoAppService;
        private readonly IAlumnoAppService _alumnoAppService;
        private readonly IGradoAppService _gradoAppService;
        public ReciboController(IWebHostEnvironment environment, IPagoAppService pagoAppService, IAlumnoAppService alumnoAppService, IGradoAppService gradoAppService)
        {
            _environment = environment;
            _pagoAppService = pagoAppService;
            _alumnoAppService = alumnoAppService;
            _gradoAppService = gradoAppService;
        }

        [HttpGet]
        public async Task<IActionResult> ImprimirPago(int pagoId)
        {
            string path = System.IO.Path.Combine(_environment.ContentRootPath, "Formatos/recibo.rdlc");
            Stream reportDefinition = System.IO.File.OpenRead(path);

            LocalReport report = new LocalReport();
            report.EnableExternalImages = true;
            report.LoadReportDefinition(reportDefinition);

            var pago = await _pagoAppService.GetPagoAsync(pagoId);
            if (pago != null)
            {

                var nombreAlumno = pago.Alumno.Nombre + pago.Alumno.Paterno + pago.Alumno.Materno;
                var nombreAlumnoSeparado = pago.Alumno.Nombre + " " + pago.Alumno.Paterno + " " + pago.Alumno.Materno;
                var grado = pago.Alumno.Grado.Fase + "° de " + pago.Alumno.Grado.Nivel;
                var fechaActual = DateTime.Now;

                byte[] streamBytes = null;
                string mimeType = "";
                string encoding = "";
                string fileNameExtension = "";
                string reportName = "Pago_"+nombreAlumno+"_"+new DateOnly(fechaActual.Year,fechaActual.Month,fechaActual.Day);
                string[] streamids = null;
                Warning[] warnings = null;

                report.SetParameters(new ReportParameter[] 
                { 
                    new ReportParameter("Folio", pago.Folio), 
                    new ReportParameter("Referencia", pago.Referencia),
                    new ReportParameter("Monto", pago.Monto.ToString()),
                    new ReportParameter("Modalidad", pago.Alumno.Modalidad.ToString()),
                    new ReportParameter("FechaPago", pago.FechaPago.ToString()),
                    new ReportParameter("Nombre", nombreAlumnoSeparado),
                    new ReportParameter("Matricula", pago.Alumno.Matricula),
                    new ReportParameter("Grado", grado),
                });

                streamBytes = report.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streamids, out warnings);

                return File(streamBytes, mimeType, $"{reportName}.{fileNameExtension}");
            }
            else
            {
                var model = new ImprimirPagoVM();
                model.PagoId = pagoId;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ImprimirPagoHTML(int pagoId)
        {
            var pago = await _pagoAppService.GetPagoAsync(pagoId);
            if (pago != null)
            {
                //return View(pago);
                return new ViewAsPdf("ImprimirPagoHTML", pago)
                {
                    PageOrientation = Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                    PageMargins = new Margins(1, 1, 2, 1),

                };
            }
            else
            {
                var model = new Pago();
                model.Id = pagoId;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ImprimirListaVertical(int gradoId, string tipoLista)
        {
            string path = System.IO.Path.Combine(_environment.ContentRootPath, "Formatos/listadoVertical.rdlc");
            Stream reportDefinition = System.IO.File.OpenRead(path);

            LocalReport report = new LocalReport();
            report.EnableExternalImages = true;
            report.LoadReportDefinition(reportDefinition);

            AlumnosDataSet dataSet = new AlumnosDataSet();
            var alumnosGrupo = await _alumnoAppService.GetAlumnosAsync(gradoId);
            var grado = await _gradoAppService.GetGradoAsync(gradoId);
            if (alumnosGrupo != null && alumnosGrupo.Count > 0 && grado != null)
            {
                int cont = 1;
                foreach (var alumno in alumnosGrupo)
                {
                    AlumnosDataSet.AlumnosRow row = dataSet.Alumnos.NewAlumnosRow();
                    row.No = cont;
                    row.Nombre = alumno.Paterno + " " + alumno.Materno + " " + alumno.Nombre;
                    dataSet.Alumnos.Rows.Add(row);
                    cont++;
                }

                var fechaActual = DateTime.Now;
                byte[] streamBytes = null;
                string mimeType = "";
                string encoding = "";
                string fileNameExtension = "";
                string reportName = "ListaAlumnos_" + grado.Fase + "_" + grado.Nivel;
                string[] streamids = null;
                Warning[] warnings = null;

                report.SetParameters(new ReportParameter[] 
                { 
                    new ReportParameter("Seccion", grado.Nivel.ToUpper()), 
                    new ReportParameter("Ciclo", (fechaActual.Year-1) + "-" + fechaActual.Year),
                    new ReportParameter("Lista", tipoLista.ToUpper()),
                    new ReportParameter("Grado", grado.Fase + "° de " + grado.Nivel.ToUpper()),
                });
                report.DataSources.Add(new ReportDataSource("Alumnos", (System.Data.DataTable)dataSet.Alumnos));

                streamBytes = report.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streamids, out warnings);

                return File(streamBytes, mimeType, $"{reportName}.{fileNameExtension}");
            }else
            {
                var model = new ImprimirListadoVM();
                model.gradoId = gradoId;
                return View(model);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> ImprimirListaHorizontal(int gradoId, string tipoLista)
        {
            string path = System.IO.Path.Combine(_environment.ContentRootPath, "Formatos/listadoHorizontal.rdlc");
            Stream reportDefinition = System.IO.File.OpenRead(path);

            LocalReport report = new LocalReport();
            report.EnableExternalImages = true;
            report.LoadReportDefinition(reportDefinition);

            AlumnosDataSet dataSet = new AlumnosDataSet();
            var alumnosGrupo = await _alumnoAppService.GetAlumnosAsync(gradoId);
            var grado = await _gradoAppService.GetGradoAsync(gradoId);
            if (alumnosGrupo != null && alumnosGrupo.Count > 0 && grado != null)
            {
                int cont = 1;
                foreach (var alumno in alumnosGrupo)
                {
                    AlumnosDataSet.AlumnosRow row = dataSet.Alumnos.NewAlumnosRow();
                    row.No = cont;
                    row.Nombre = alumno.Paterno + " " + alumno.Materno + " " + alumno.Nombre;
                    dataSet.Alumnos.Rows.Add(row);
                    cont++;
                }

                var fechaActual = DateTime.Now;
                byte[] streamBytes = null;
                string mimeType = "";
                string encoding = "";
                string fileNameExtension = "";
                string reportName = "ListaAlumnos_" + grado.Fase + "_" + grado.Nivel;
                string[] streamids = null;
                Warning[] warnings = null;

                report.SetParameters(new ReportParameter[] 
                { 
                    new ReportParameter("Seccion", grado.Nivel.ToUpper()), 
                    new ReportParameter("Ciclo", (fechaActual.Year-1) + "-" + fechaActual.Year),
                    new ReportParameter("Lista", tipoLista.ToUpper()),
                    new ReportParameter("Grado", grado.Fase + "° de " + grado.Nivel.ToUpper()),
                });
                report.DataSources.Add(new ReportDataSource("Alumnos", (System.Data.DataTable)dataSet.Alumnos));

                streamBytes = report.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streamids, out warnings);

                return File(streamBytes, mimeType, $"{reportName}.{fileNameExtension}");
            }else
            {
                var model = new ImprimirListadoVM();
                model.gradoId = gradoId;
                return View(model);
            }
        }
    }
}
