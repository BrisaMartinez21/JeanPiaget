using JeanPiaget.ApplicationServices.Alumnos;
using JeanPiaget.ApplicationServices.Materias;
using JeanPiaget.Core.DTOs.Materias;
using JeanPiaget.Core.Materias;
using JeanPiaget.Web.Models.Materias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeanPiaget.Web.Controllers
{
    [Authorize(Roles = "Root,Direccion")]
    public class MateriaController : Controller
    {
        private readonly IGradoAppService _gradoAppService;
        private readonly IMateriaAppService _materiaAppService;
        private readonly IAlumnoAppService _alumnoAppService;

        public MateriaController(IGradoAppService gradoAppService, IMateriaAppService materiaAppService, IAlumnoAppService alumnoAppService) 
        {
            _gradoAppService = gradoAppService;
            _materiaAppService = materiaAppService;
            _alumnoAppService = alumnoAppService;
        }
        public async Task<ActionResult> Index(int? pagina, string? buscarItem, int? filtroBusqueda)
        {
            int cantidadRegistros = 10;
            MateriaVMPaginacion modelo = new MateriaVMPaginacion();
            if (!string.IsNullOrEmpty(buscarItem) && filtroBusqueda.HasValue)
            {
                if (pagina.HasValue)
                {
                    modelo.Materias = await _materiaAppService.GetMateriasFilterAsync(buscarItem, filtroBusqueda.Value, pagina.Value, cantidadRegistros);
                    modelo.PaginaActual = pagina.Value;
                    modelo.TotalDeRegistros = await _materiaAppService.TotalMateriasFilterAsync(buscarItem, filtroBusqueda.Value);
                    modelo.BuscarItem = buscarItem;
                    modelo.FiltroBusqueda = filtroBusqueda.Value;
                    modelo.EsBuscador = true;
                }
                else
                {
                    modelo.Materias = await _materiaAppService.GetMateriasFilterAsync(buscarItem, filtroBusqueda.Value, 1, cantidadRegistros);
                    modelo.PaginaActual = 1;
                    modelo.TotalDeRegistros = await _materiaAppService.TotalMateriasFilterAsync(buscarItem, filtroBusqueda.Value);
                    modelo.BuscarItem = buscarItem;
                    modelo.FiltroBusqueda = filtroBusqueda.Value;
                    modelo.EsBuscador = true;
                }
            }
            else
            {
                if (!pagina.HasValue)
                {
                    modelo.Materias = await _materiaAppService.PaginationMaterias(1, cantidadRegistros);
                    modelo.PaginaActual = 1;
                }
                else
                {
                    modelo.Materias = await _materiaAppService.PaginationMaterias(pagina.Value, cantidadRegistros);
                    modelo.PaginaActual = pagina.Value;
                }
                modelo.TotalDeRegistros = await _materiaAppService.TotalMaterias();
                modelo.EsBuscador = false;
            }
            modelo.RegistrosPorPagina = cantidadRegistros;

            return View(modelo);
        }

        public async Task<ActionResult> Create()
        {
            MateriaVM modelo = new MateriaVM()
            {
                Grados = await _gradoAppService.GetGradosNames(),
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<ActionResult> Create(MateriaVM model)
        {
            try
            {
                if (ModelState.IsValid && model.GradoId != 0)
                {
                    int digitosYear = DateTime.Now.Year % 100;
                    var grado = await _gradoAppService.GetGradoAsync(model.GradoId);
                    var clave = "";
                    var nivel = 0;
                    string primerosTres = model.Nombre.Substring(0, 3);
                    switch (grado.Nivel)
                    {
                        case "Kinder":
                            nivel = 1; break;
                        case "Primaria":
                            nivel = 2; break;
                        case "Secundaria":
                            nivel = 3; break;
                        case "Preparatoria":
                            nivel = 4; break;
                    }
                    Random random = new Random();
                    bool salida = true;
                    while (salida)
                    {
                        int parteClave = random.Next(10, 99);
                        clave = "1602" + digitosYear + primerosTres.ToUpper() + nivel + grado.Fase + parteClave;
                        if (!await _materiaAppService.ExisteClaveAsync(clave))
                        {
                            salida = false; break;
                        }
                    }
                    var materia = new Materia()
                    {
                        Nombre = model.Nombre,
                        Clave = clave,
                        Grado = grado,
                    };

                    await _materiaAppService.AddMateriaAsync(materia);

                    return RedirectToAction("Index");
                }else
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
            var materia = await _materiaAppService.GetMateriaAsync(id);
            MateriaVM modelo = new MateriaVM()
            {
                Id = id,
                Nombre = materia.Nombre,
                GradoId = materia.Grado.Id,
                Grados = await _gradoAppService.GetGradosNames(),
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MateriaVM model)
        {
            try
            {
                if (ModelState.IsValid && model.GradoId != 0 && model.Id.HasValue)
                {
                    var materia = await _materiaAppService.GetMateriaAsync(model.Id.Value);
                    var clave = "";
                    var nivel = 0;
                    int digitosYear = DateTime.Now.Year % 100;
                    string primerosTres = model.Nombre.Substring(0, 3);
                    var grado = await _gradoAppService.GetGradoAsync(model.GradoId);

                    switch (grado.Nivel)
                    {
                        case "Kinder":
                            nivel = 1; break;
                        case "Primaria":
                            nivel = 2; break;
                        case "Secundaria":
                            nivel = 3; break;
                        case "Preparatoria":
                            nivel = 4; break;
                    }
                    Random random = new Random();
                    bool salida = true;
                    while (salida)
                    {
                        int parteClave = random.Next(10, 99);
                        clave = "1602" + digitosYear + primerosTres.ToUpper() + nivel + grado.Fase + parteClave;
                        if (!await _materiaAppService.ExisteClaveAsync(clave))
                        {
                            salida = false; break;
                        }
                    }

                    materia.Nombre = model.Nombre;
                    materia.Clave = clave;
                    materia.Grado = await _gradoAppService.GetGradoAsync(model.GradoId);

                    await _materiaAppService.EditMateriaAsync(materia);

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
            var materia = await _materiaAppService.GetMateriaAsync(id);
            if (materia != null)
            {
                await _materiaAppService.DeleteMateriaAsync(id);
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Grados()
        {
            var grados = await _gradoAppService.GetGradosAsync();
            List<GradoCantidadDTO> lista = new List<GradoCantidadDTO>();
            foreach (var grado in grados)
            {
                lista.Add(new GradoCantidadDTO
                {
                    Grado = grado,
                    Cantidad = await _alumnoAppService.GetTotalAlumnosGradoAsync(grado.Id),
                });
            }
            GradosVM model = new GradosVM()
            {
                Grados = lista,
            };
            return View(model);
        }
    }
}
