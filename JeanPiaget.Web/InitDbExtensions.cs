using JeanPiaget.ApplicationServices.Alumnos;
using JeanPiaget.ApplicationServices.Materias;
using JeanPiaget.Core.Usuarios;
using JeanPiaget.Web;
using Microsoft.AspNetCore.Identity;

namespace JeanPiaget.Web
{
    public static class InitDbExtensions
    {
        public static IApplicationBuilder InitDB(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetService<UserManager<IdentityUser>>();
                var roleManager = services.GetService<RoleManager<IdentityRole>>();
                var gradoAppService = services.GetService<IGradoAppService>();

                if (userManager.Users.Count() == 0)
                {
                    Task.Run(() => InitRoles(roleManager)).Wait();
                    Task.Run(() => InitUsers(userManager)).Wait();
                    Task.Run(() => InitGrados(gradoAppService)).Wait();
                }

                return app;
            }
        }

        private static async Task InitRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var roles = Constantes.Roles;
                foreach(var role in roles)
                {
                    var newRole = new IdentityRole(role);
                    await roleManager.CreateAsync(newRole);
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private static async Task InitUsers(UserManager<IdentityUser> userManager)
        {
            var role = Constantes.Roles[0];
            var userRoot = Constantes.UsuarioRoot;
            var fechaActual = DateTime.Now;
            var user = new Usuario()
            {
                Nombre = userRoot["Nombre"],
                Cargo = userRoot["Cargo"],
                FechaCreacion = fechaActual,
                FechaNacimiento = new DateOnly(fechaActual.Year,fechaActual.Month,fechaActual.Day),
                UserName = userRoot["UserName"],
                Email = userRoot["Email"],
                PhoneNumber = userRoot["PhoneNumber"],
            };
            await userManager.CreateAsync(user, userRoot["Password"]);
            await userManager.AddToRoleAsync(user, role);
        }
        
        private static async Task InitGrados(IGradoAppService gradoAppService)
        {
            var grados = Constantes.Grados;
            foreach (var grado in grados)
            {
                await gradoAppService.AddGradoAsync(grado);
            }
        }

    }
}
