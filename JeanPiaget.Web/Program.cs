using JeanPiaget.Web;
using JeanPiaget.Web.Mappers;
using JeanPiaget.ApplicationServices.Alumnos;
using JeanPiaget.ApplicationServices.Calificaciones;
using JeanPiaget.ApplicationServices.Materias;
using JeanPiaget.ApplicationServices.Pagos;
using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.Calificaciones;
using JeanPiaget.Core.Materias;
using JeanPiaget.Core.Pagos;
using JeanPiaget.DataAccess;
using JeanPiaget.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JeanPiaget.ApplicationServices.Usuarios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<JeanPiagetContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<JeanPiagetContext>()
  .AddDefaultTokenProviders()
  .AddDefaultUI();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/logIn");

builder.Services.AddScoped<IGradoAppService, GradoAppService>();
builder.Services.AddScoped<IRepository<int, Grado>, Repository<int, Grado>>();

builder.Services.AddScoped<IAlumnoAppService, AlumnoAppService>();
builder.Services.AddScoped<IRepository<int, Alumno>, Repository<int, Alumno>>();

/* builder.Services.AddScoped<IBecaAppService, BecaAppService>();
builder.Services.AddScoped<IRepository<int, Beca>, Repository<int, Beca>>(); */

builder.Services.AddScoped<ITutorAppService, TutorAppService>();
builder.Services.AddScoped<IRepository<int, Tutor>, Repository<int, Tutor>>();

builder.Services.AddScoped<ICalificacionAppService, CalificacionAppService>();
builder.Services.AddScoped<IRepository<int, Calificacion>, Repository<int, Calificacion>>();

builder.Services.AddScoped<IMateriaAppService, MateriaAppService>();
builder.Services.AddScoped<IRepository<int, Materia>, Repository<int, Materia>>();

builder.Services.AddScoped<IPagoAppService, PagoAppService>();
builder.Services.AddScoped<IRepository<int, Pago>, Repository<int, Pago>>();

builder.Services.AddScoped<IUsuarioAppService, UsuarioAppService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.InitDB();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa/Windows");

app.Run();
