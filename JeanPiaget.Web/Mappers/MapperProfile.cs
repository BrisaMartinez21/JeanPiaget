using AutoMapper;
using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.Calificaciones;
using JeanPiaget.Core.DTOs.Alumnos;
using JeanPiaget.Core.DTOs.Calificaciones;
using JeanPiaget.Core.DTOs.Materias;
using JeanPiaget.Core.DTOs.Pagos;
using JeanPiaget.Core.DTOs.Usuarios;
using JeanPiaget.Core.Materias;
using JeanPiaget.Core.Pagos;
using JeanPiaget.Core.Usuarios;
using Microsoft.AspNetCore.Identity;

namespace JeanPiaget.Web.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Grado, GradoDTO>().ReverseMap();
            CreateMap<Materia, MateriaDTO>().ReverseMap();
            CreateMap<Materia, MateriaListDTO>().ReverseMap();
            CreateMap<Beca, BecaDTO>().ReverseMap();
            CreateMap<Alumno, AlumnoDTO>().ReverseMap();
            CreateMap<Alumno, AlumnoListDTO>().ReverseMap();
            CreateMap<Tutor, TutorDTO>().ReverseMap();
            CreateMap<Tutor, TutorListDTO>().ReverseMap();
            CreateMap<Calificacion, CalificacionDTO>().ReverseMap();
            CreateMap<Calificacion, CalificacionListDTO>().ReverseMap();
            CreateMap<Pago, PagoDTO>().ReverseMap();
            CreateMap<Pago, PagoAddDTO>().ReverseMap();
            CreateMap<Pago, PagoListDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioAddDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioUpdateDTO>().ReverseMap();
            CreateMap<Usuario, IdentityUser>().ReverseMap();
            CreateMap<IdentityUser, Usuario>().ReverseMap();
            CreateMap<RolesDTO, IdentityRole>().ReverseMap();
        }
    }
}
