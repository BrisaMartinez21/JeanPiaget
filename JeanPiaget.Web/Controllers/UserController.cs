using AutoMapper;
using JeanPiaget.ApplicationServices.Usuarios;
using JeanPiaget.Core.DTOs.Usuarios;
using JeanPiaget.Core.Usuarios;
using JeanPiaget.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System.Data;

namespace JeanPiaget.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUsuarioAppService _usuarioAppService;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUsuarioAppService usuarioAppService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _usuarioAppService = usuarioAppService;
        }

        [Authorize(Roles = "Root")]
        public async Task<ActionResult> Index(int? pagina, string? buscarItem, int? filtroBusqueda)
        {
            int cantidadRegistros = 10;
            UserVMPaginacion modelo = new UserVMPaginacion();
            var userIgnore = Constantes.UsuarioRoot;
            if (!string.IsNullOrEmpty(buscarItem) && filtroBusqueda.HasValue)
            {
                if (pagina.HasValue)
                {
                    modelo.Usuarios = await _usuarioAppService.GetUsuarioFilterAsync(buscarItem, filtroBusqueda.Value, pagina.Value, cantidadRegistros, userIgnore);
                    modelo.PaginaActual = pagina.Value;
                    modelo.TotalDeRegistros = await _usuarioAppService.TotaUsuarioFilterAsync(buscarItem, filtroBusqueda.Value, userIgnore);
                    modelo.BuscarItem = buscarItem;
                    modelo.FiltroBusqueda = filtroBusqueda.Value;
                    modelo.EsBuscador = true;
                }
                else
                {
                    modelo.Usuarios = await _usuarioAppService.GetUsuarioFilterAsync(buscarItem, filtroBusqueda.Value, 1, cantidadRegistros, userIgnore);
                    modelo.PaginaActual = 1;
                    modelo.TotalDeRegistros = await _usuarioAppService.TotaUsuarioFilterAsync(buscarItem, filtroBusqueda.Value, userIgnore);
                    modelo.BuscarItem = buscarItem;
                    modelo.FiltroBusqueda = filtroBusqueda.Value;
                    modelo.EsBuscador = true;
                }
            }
            else
            {
                if (!pagina.HasValue)
                {
                    modelo.Usuarios = await _usuarioAppService.PaginationUsuario(1, cantidadRegistros, userIgnore);
                    modelo.PaginaActual = 1;
                }
                else
                {
                    modelo.Usuarios = await _usuarioAppService.PaginationUsuario(pagina.Value, cantidadRegistros, userIgnore);
                    modelo.PaginaActual = pagina.Value;
                }
                modelo.TotalDeRegistros = await _usuarioAppService.TotalUsuarios(userIgnore);
                modelo.EsBuscador = false;
            }
            modelo.RegistrosPorPagina = cantidadRegistros;

            return View(modelo);
        }

        [Authorize(Roles = "Root,Administrador,Direccion")]
        public async Task<IActionResult> Descripcion(string name)
        {
            var identityUser = await _usuarioAppService.GetUsuarioByNameAsync(name);
            if (identityUser != null)
            {
                UsuarioRolDTO usuario = new UsuarioRolDTO()
                {
                    Usuario = identityUser,
                };

                var rolesUsuario = await _userManager.GetRolesAsync(identityUser);
                if (rolesUsuario != null)
                {
                    usuario.Role = await _roleManager.FindByNameAsync(rolesUsuario[0]);
                }
                return View(usuario);
            }
            return View(null);
        }

        [Authorize(Roles = "Root")]
        public async Task<ActionResult> Create()
        {
            var fechaActual = DateTime.Now;
            UserVM model = new UserVM()
            {
                FechaNacimiento = new DateOnly(fechaActual.Year, fechaActual.Month, fechaActual.Day),
            };
            return View(model);
        }

        [Authorize(Roles = "Root")]
        [HttpPost]
        public async Task<ActionResult> Create(UserVM model)
        {
            try
            {
                if (ModelState.IsValid && model.Rol != "")
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "¡El correo electrónico ya está en uso!");

                        if (model.Password != model.ConfirmPassword)
                        {
                            ModelState.AddModelError("Password", "¡La contraseña y la confirmación no son iguales!");
                        }
                        return View(model);
                    }
                    else
                    {
                        if (model.Password != model.ConfirmPassword)
                        {
                            ModelState.AddModelError("Password", "¡La contraseña y la confirmación no son iguales!");
                            return View(model);
                        }
                        var newUser = new Usuario()
                        {
                            Nombre = model.Nombre,
                            Paterno = model.Paterno,
                            Materno = model.Materno,
                            Genero = model.Genero,
                            Cargo = model.Cargo,
                            FechaCreacion = DateTime.Now,
                            FechaNacimiento = model.FechaNacimiento,
                            UserName = model.Email,
                            Email = model.Email,
                            PhoneNumber = model.Telefono,
                        };


                        await _userManager.CreateAsync(newUser, model.Password);
                        await _userManager.AddToRoleAsync(newUser, model.Rol);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                return View(model);
            }
        }

        [Authorize(Roles = "Root")]
        public async Task<IActionResult> Edit(string id)
        {
            var usuario = await _usuarioAppService.GetUsuarioAsync(id);
            if(usuario != null)
            {
                var Rol = new IdentityRole();
                var rolesUsuario = await _userManager.GetRolesAsync(usuario);
                if (rolesUsuario != null)
                {
                    Rol = await _roleManager.FindByNameAsync(rolesUsuario[0]);
                }
                UserEditVM model = new UserEditVM()
                {
                    Id = id,
                    Nombre = usuario.Nombre,
                    Paterno = usuario.Paterno,
                    Materno = usuario.Materno,
                    Genero = usuario.Genero,
                    FechaNacimiento = usuario.FechaNacimiento,
                    Cargo = usuario.Cargo,
                    Rol = Rol!=null && Rol.Name!=null ? Rol.Name : "",
                    Telefono = usuario.PhoneNumber,
                    Email = usuario.Email,
                };
                return View(model);
            }
            return View(null);
        }

        [Authorize(Roles = "Root")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            try
            {
                if (ModelState.IsValid && model.Id != null)
                {
                    var usuario = await _usuarioAppService.GetUsuarioAsync(model.Id);
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null && usuario != null && usuario.Email!=model.Email)
                    {
                        ModelState.AddModelError("Email", "¡El correo electrónico ya está en uso!");

                        if ((model.Password != null || model.ConfirmPassword != null) && model.Password != model.ConfirmPassword)
                        {
                            ModelState.AddModelError("Password", "¡La contraseña y la confirmación no son iguales!");
                        }
                        return View(model);
                    }
                    else
                    {
                        if (usuario != null)
                        {
                            usuario.Nombre = model.Nombre;
                            usuario.Paterno = model.Paterno;
                            usuario.Materno = model.Materno;
                            usuario.Genero = model.Genero;
                            usuario.FechaNacimiento = model.FechaNacimiento;
                            usuario.Cargo = model.Cargo;
                            usuario.PhoneNumber = model.Telefono;
                            usuario.Email = model.Email;
                            usuario.UserName = model.Email;
                            var rolesUsuario = await _userManager.GetRolesAsync(usuario);
                            var update = await _userManager.UpdateAsync(usuario);

                            if (rolesUsuario != null && rolesUsuario[0] != model.Rol)
                            {
                                await _userManager.RemoveFromRolesAsync(usuario, rolesUsuario);
                                await _userManager.AddToRoleAsync(usuario, model.Rol);
                            }
                            if (model.Password != null || model.ConfirmPassword != null)
                            {
                                if(model.Password != model.ConfirmPassword)
                                {
                                    ModelState.AddModelError("Password", "¡La contraseña y la confirmación no son iguales!");
                                    return View(model);
                                }
                                else if (model.ConfirmPassword == model.Password)
                                {
                                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                                    var resultado = await _userManager.ResetPasswordAsync(usuario, token, $"{model.Password}");
                                }
                            }
                        }
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                return View(model);
            }
        }

        [Authorize(Roles = "Root")]
        public async Task<ActionResult> Delete(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                var resultado = await _userManager.DeleteAsync(usuario);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
