using JeanPiaget.ApplicationServices.Alumnos;
using JeanPiaget.ApplicationServices.Materias;
using JeanPiaget.Core.Alumnos;
using JeanPiaget.Web.Models.Alumnos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeanPiaget.Web.Controllers
{
    [Authorize(Roles = "Root,Administrador")]
    public class AlumnoController : Controller
    {
        private readonly IAlumnoAppService _alumnoAppService;
        private readonly ITutorAppService _tutorAppService;
        private readonly IGradoAppService _gradoAppService;
        private readonly IMateriaAppService _materiaAppService;

        public AlumnoController(IAlumnoAppService alumnoAppService, ITutorAppService tutorAppService, IGradoAppService gradoAppService,IMateriaAppService materiaAppService)
        {
            _alumnoAppService = alumnoAppService;
            _tutorAppService = tutorAppService;
            _gradoAppService = gradoAppService;
            _materiaAppService = materiaAppService;
        }

        public async Task<ActionResult> Index(int? pagina, string? buscarItem, int? filtroBusqueda)
        {
            int cantidadRegistros = 25;
            AlumnoVMPaginacion modelo = new AlumnoVMPaginacion();
            if (!string.IsNullOrEmpty(buscarItem) && filtroBusqueda.HasValue)
            {
                if (pagina.HasValue)
                {
                    modelo.Alumnos = await _alumnoAppService.GetAlumnosFilterAsync(buscarItem, filtroBusqueda.Value, pagina.Value, cantidadRegistros);
                    modelo.PaginaActual = pagina.Value;
                    modelo.TotalDeRegistros = await _alumnoAppService.TotaAlumnosFilterAsync(buscarItem, filtroBusqueda.Value);
                    modelo.BuscarItem = buscarItem;
                    modelo.FiltroBusqueda = filtroBusqueda.Value;
                    modelo.EsBuscador = true;
                }
                else
                {
                    modelo.Alumnos = await _alumnoAppService.GetAlumnosFilterAsync(buscarItem, filtroBusqueda.Value, 1, cantidadRegistros);
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
                    modelo.Alumnos = await _alumnoAppService.PaginationAlumnos(1, cantidadRegistros);
                    modelo.PaginaActual = 1;
                }
                else
                {
                    modelo.Alumnos = await _alumnoAppService.PaginationAlumnos(pagina.Value, cantidadRegistros);
                    modelo.PaginaActual = pagina.Value;
                }
                modelo.TotalDeRegistros = await _alumnoAppService.TotalAlumnos();
                modelo.EsBuscador = false;
            }
            modelo.RegistrosPorPagina = cantidadRegistros;

            return View(modelo);
        }

        public async Task<ActionResult> Create()
        {
            var fechaActual = DateTime.Now;
            AlumnoVM model = new AlumnoVM()
            {
                Grados = await _gradoAppService.GetGradosNames(),
                FechaNacimiento = new DateOnly(fechaActual.Year, fechaActual.Month, fechaActual.Day),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AlumnoVM model)
        {
            try
            {
                if (ModelState.IsValid && model.GradoId != 0)
                {
                    var alumno = new Alumno();
                    if (model.Beca.HasValue && model.Beca != 0) //Agregar un alumno con beca
                    {
                        alumno = new Alumno()
                        {
                            Nombre = model.Nombre,
                            Paterno = model.Paterno,
                            Materno = model.Materno,
                            FechaNacimiento = model.FechaNacimiento,
                            Mensualidad = model.Mensualidad,
                            Modalidad = model.Modalidad,
                            Genero = model.Genero,
                            Grado = await _gradoAppService.GetGradoAsync(model.GradoId),
                            Beca = model.Beca.Value,
                        };
                    }else
                    {
                        alumno = new Alumno()
                        {
                            Nombre = model.Nombre,
                            Paterno = model.Paterno,
                            Materno = model.Materno,
                            FechaNacimiento = model.FechaNacimiento,
                            Mensualidad = model.Mensualidad,
                            Modalidad = model.Modalidad,
                            Genero = model.Genero,
                            Grado = await _gradoAppService.GetGradoAsync(model.GradoId),
                        };
                    }
                    Random random = new Random();
                    bool salida = true;
                    int digitosYear = DateTime.Now.Year % 100;
                    while (salida)
                    {
                        int parteMatricula = random.Next(1110, 9999);
                        alumno.Matricula = "1602" + digitosYear + parteMatricula;
                        if (!await _alumnoAppService.ExisteMatriculaAsync(alumno.Matricula))
                        {
                            salida = false; break;
                        }
                    }

                    var tutor = new Tutor()
                    {
                        Nombre = model.NombreTutor,
                        Paterno = model.PaternoTutor,
                        Materno = model.MaternoTutor,
                        Telefono = model.TelefonoTutor,
                        Correo = model.CorreoElectronico,
                        Genero = model.GeneroTutor,
                    };
                    var tutorId = await _tutorAppService.AddTutorAsync(tutor);
                    var newTutor = await _tutorAppService.GetTutorAsync(tutorId);
                    alumno.Tutor = newTutor;

                    var alumnoId = await _alumnoAppService.AddAlumnoAsync(alumno);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("GradoId", "¡Selecciona un grado!");
                    model.Grados = await _gradoAppService.GetGradosNames();
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                model.Grados = await _gradoAppService.GetGradosNames();
                return View(model);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var alumno = await _alumnoAppService.GetAlumnoAsync(id);
            AlumnoVM model = new AlumnoVM()
            {
                Id = id,
                Nombre = alumno.Nombre,
                Paterno = alumno.Paterno,
                Materno = alumno.Materno,
                FechaNacimiento = alumno.FechaNacimiento,
                Mensualidad = alumno.Mensualidad,
                Modalidad = alumno.Modalidad,
                Genero = alumno.Genero,
                GradoId = alumno.Grado.Id,
                Beca = alumno.Beca,
                Grados = await _gradoAppService.GetGradosNames(),
                TutorId = alumno.Tutor.Id,
                NombreTutor = alumno.Tutor.Nombre,
                PaternoTutor = alumno.Tutor.Paterno,
                MaternoTutor = alumno.Tutor.Materno,
                TelefonoTutor = alumno.Tutor.Telefono,
                CorreoElectronico = alumno.Tutor.Correo,
                GeneroTutor = alumno.Tutor.Genero,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AlumnoVM model)
        {
            try
            {
                if (ModelState.IsValid && model.GradoId != 0 && model.Id.HasValue && model.TutorId.HasValue)
                {
                    var alumno = await _alumnoAppService.GetAlumnoAsync(model.Id.Value);
                    if (model.Beca.HasValue && model.Beca != 0) //Editar un alumno con beca
                    {
                        alumno.Nombre = model.Nombre;
                        alumno.Paterno = model.Paterno;
                        alumno.Materno = model.Materno;
                        alumno.FechaNacimiento = model.FechaNacimiento;
                        alumno.Mensualidad = model.Mensualidad;
                        alumno.Modalidad = model.Modalidad;
                        alumno.Genero = model.Genero;
                        alumno.Grado = await _gradoAppService.GetGradoAsync(model.GradoId);
                        alumno.Beca = model.Beca.Value;
                    }
                    else
                    {
                        alumno.Nombre = model.Nombre;
                        alumno.Paterno = model.Paterno;
                        alumno.Materno = model.Materno;
                        alumno.FechaNacimiento = model.FechaNacimiento;
                        alumno.Mensualidad = model.Mensualidad;
                        alumno.Modalidad = model.Modalidad;
                        alumno.Genero = model.Genero;
                        alumno.Grado = await _gradoAppService.GetGradoAsync(model.GradoId);
                        alumno.Beca = null;
                    }

                    var tutor = await _tutorAppService.GetTutorAsync(model.TutorId.Value);
                    tutor.Nombre = model.NombreTutor;
                    tutor.Paterno = model.PaternoTutor;
                    tutor.Materno = model.MaternoTutor;
                    tutor.Telefono = model.TelefonoTutor;
                    tutor.Correo = model.CorreoElectronico;
                    tutor.Genero = model.GeneroTutor;

                    await _tutorAppService.EditTutorAsync(tutor);
                    var updateTutor = await _tutorAppService.GetTutorAsync(tutor.Id);
                    alumno.Tutor = updateTutor;

                    await _alumnoAppService.EditAlumnoAsync(alumno);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("GradoId", "¡Selecciona un grado!");
                    model.Grados = await _gradoAppService.GetGradosNames();
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                model.Grados = await _gradoAppService.GetGradosNames();
                return View(model);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var alumno = await _alumnoAppService.GetAlumnoAsync(id);
            if (alumno != null)
            {
                var tutorId = alumno.Tutor.Id;
                await _alumnoAppService.DeleteAlumnoAsync(id);
                await _tutorAppService.DeleteTutorAsync(tutorId);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerCalificaciones(int id)
        {
            var alumno = await _alumnoAppService.GetAlumnoAsync(id);
            if (alumno != null)
            {
                VerCalificacionesVM model = new VerCalificacionesVM()
                {
                    Alumno = alumno,
                    Materias = await _materiaAppService.GetMateriasByGradoAsync(alumno.Grado.Id),
                };
                return View(model);
            }
            return View(null);
        }
    }
}
