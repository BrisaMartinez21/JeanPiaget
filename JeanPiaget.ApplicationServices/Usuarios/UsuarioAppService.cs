using JeanPiaget.Core.DTOs.Usuarios;
using JeanPiaget.Core.Usuarios;
using JeanPiaget.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Usuarios
{
    public class UsuarioAppService : IUsuarioAppService
    {
        public readonly JeanPiagetContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        protected JeanPiagetContext Context { get => _context; }
        public UsuarioAppService(JeanPiagetContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ValueTask<Usuario> GetUsuarioAsync(string usuarioId)
        {
            var usuario = _context.Usuarios.FindAsync(usuarioId);
            return usuario;
        }

        public async Task<Usuario> GetUsuarioByNameAsync(string name)
        {
            var identityUser = await _userManager.FindByNameAsync(name);
            if (identityUser != null)
            {
                var user = (Usuario) identityUser;
                return user;
            }
            return null;
        }

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            if (identityUser != null)
            {
                var user = (Usuario) identityUser;
                return user;
            }
            return null;
        }

        public async Task EditUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UsuarioRolDTO>> GetUsuarioFilterAsync(string busqueda, int filtro, int pagina, int cantidad, Dictionary<string, string> userIgnore)
        {
            switch (filtro)
            {
                case 1:
                    var list = await _userManager.Users.ToListAsync();
                    List<UsuarioRolDTO> usuarios = new List<UsuarioRolDTO>();
                    if (list != null)
                    {
                        foreach (var user in list)
                        {
                            if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                            {
                                var userAdd = new UsuarioRolDTO { Usuario = (Usuario)user };
                                var rolesUsuario = await _userManager.GetRolesAsync(user);
                                if (rolesUsuario != null)
                                {
                                    userAdd.Role = await _roleManager.FindByNameAsync(rolesUsuario[0]);
                                }
                                usuarios.Add(userAdd);
                            }
                        }
                    }
                    return usuarios.Where(x => (x.Usuario.Nombre.ToLower() + " " + x.Usuario.Paterno.ToLower() + " " + x.Usuario.Materno.ToLower())
                        .Contains(busqueda.ToLower()))
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToList();
                    break;
                case 2:
                    var list2 = await _userManager.Users.ToListAsync();
                    List<UsuarioRolDTO> usuarios2 = new List<UsuarioRolDTO>();
                    if (list2 != null)
                    {
                        foreach (var user in list2)
                        {
                            if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                            {
                                var userAdd = new UsuarioRolDTO { Usuario = (Usuario)user };
                                var rolesUsuario = await _userManager.GetRolesAsync(user);
                                if (rolesUsuario != null)
                                {
                                    userAdd.Role = await _roleManager.FindByNameAsync(rolesUsuario[0]);
                                }
                                usuarios2.Add(userAdd);
                            }
                        }
                    }
                    return usuarios2.Where(x => x.Usuario.Cargo.ToLower()
                        .Contains(busqueda.ToLower()))
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToList();
                    break;
                case 3:
                    var list3 = await _userManager.Users.ToListAsync();
                    List<UsuarioRolDTO> usuarios3 = new List<UsuarioRolDTO>();
                    if (list3 != null)
                    {
                        foreach (var user in list3)
                        {
                            if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                            {
                                var userAdd = new UsuarioRolDTO { Usuario = (Usuario)user };
                                var rolesUsuario = await _userManager.GetRolesAsync(user);
                                if (rolesUsuario != null)
                                {
                                    userAdd.Role = await _roleManager.FindByNameAsync(rolesUsuario[0]);
                                }
                                usuarios3.Add(userAdd);
                            }
                        }
                    }
                    return usuarios3.Where(x => x.Usuario.Email != null && x.Usuario.Email.ToLower().Contains(busqueda.ToLower()))
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToList();
                    break;
                case 4:
                    var list4 = await _userManager.Users.ToListAsync();
                    List<UsuarioRolDTO> usuarios4 = new List<UsuarioRolDTO>();
                    if (list4 != null)
                    {
                        foreach (var user in list4)
                        {
                            if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                            {
                                var userAdd = new UsuarioRolDTO { Usuario = (Usuario)user };
                                var rolesUsuario = await _userManager.GetRolesAsync(user);
                                if (rolesUsuario != null)
                                {
                                    userAdd.Role = await _roleManager.FindByNameAsync(rolesUsuario[0]);
                                }
                                usuarios4.Add(userAdd);
                            }
                        }
                    }
                    return usuarios4.Where(x => x.Usuario.PhoneNumber != null && x.Usuario.PhoneNumber.ToLower().Contains(busqueda.ToLower()))
                        .Skip((pagina - 1) * cantidad)
                        .Take(cantidad).ToList();
                    break;
            }
            return await Task.FromResult(new List<UsuarioRolDTO>());
        }

        public async Task<int> TotaUsuarioFilterAsync(string busqueda, int filtro, Dictionary<string, string> userIgnore)
        {
            switch (filtro)
            {
                case 1:
                    var list = await _userManager.Users.ToListAsync();
                    List<Usuario> usuarios = new List<Usuario>();
                    if (list != null)
                    {
                        foreach (var user in list)
                        {
                            if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                            {
                                usuarios.Add((Usuario)user);
                            }
                        }
                    }
                    return usuarios.Where(x => (x.Nombre.ToLower() + " " + x.Paterno.ToLower() + " " + x.Materno.ToLower())
                        .Contains(busqueda.ToLower())).Count();
                    break;
                case 2:
                    var list2 = await _userManager.Users.ToListAsync();
                    List<Usuario> usuarios2 = new List<Usuario>();
                    if (list2 != null)
                    {
                        foreach (var user in list2)
                        {
                            if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                            {
                                usuarios2.Add((Usuario)user);
                            }
                        }
                    }
                    return usuarios2.Where(x => x.Cargo.ToLower()
                        .Contains(busqueda.ToLower())).Count();
                    break;
                case 3:
                    var list3 = await _userManager.Users.ToListAsync();
                    List<Usuario> usuarios3 = new List<Usuario>();
                    if (list3 != null)
                    {
                        foreach (var user in list3)
                        {
                            if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                            {
                                usuarios3.Add((Usuario)user);
                            }
                        }
                    }
                    return usuarios3.Where(x => x.Email != null && x.Email.ToLower().Contains(busqueda.ToLower())).Count();
                    break;
                case 4:
                    var list4 = await _userManager.Users.ToListAsync();
                    List<Usuario> usuarios4 = new List<Usuario>();
                    if (list4 != null)
                    {
                        foreach (var user in list4)
                        {
                            if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                            {
                                usuarios4.Add((Usuario)user);
                            }
                        }
                    }
                    return usuarios4.Where(x => x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(busqueda.ToLower())).Count();
                    break;
            }
            return 0;
        }
        public async Task<List<UsuarioRolDTO>> PaginationUsuario(int pagina, int cantidad, Dictionary<string, string> userIgnore)
        {
            var list = await _userManager.Users.ToListAsync();
            List<UsuarioRolDTO> usuarios = new List<UsuarioRolDTO>();
            if (list != null)
            {
                foreach (var user in list)
                {
                    if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                    {
                        var userAdd = new UsuarioRolDTO { Usuario = (Usuario)user };
                        var rolesUsuario = await _userManager.GetRolesAsync(user);
                        if (rolesUsuario != null)
                        {
                            userAdd.Role = await _roleManager.FindByNameAsync(rolesUsuario[0]);
                        }
                        usuarios.Add(userAdd);
                    }
                }
            }
            return usuarios.Skip((pagina - 1) * cantidad).Take(cantidad).ToList();
        }

        public async Task<int> TotalUsuarios(Dictionary<string, string> userIgnore)
        {
            var list = await _userManager.Users.ToListAsync();
            List<Usuario> usuarios = new List<Usuario>();
            if (list != null)
            {
                foreach (var user in list)
                {
                    if (user.Email != userIgnore["Email"] && user.PhoneNumber != userIgnore["PhoneNumber"])
                    {
                        usuarios.Add((Usuario)user);
                    }
                }
            }
            return usuarios.Count();
        }
    }
}
