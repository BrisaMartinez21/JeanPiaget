using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.Materias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JeanPiaget.Web.Models.Alumnos
{
    public class AlumnoVM
    {
        public int? Id { get; set; }
        public int? TutorId { get; set; }
        public List<SelectListItem>? Grados { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo Nombre es requerido!")]
        public string Nombre { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo de apellido Paterno es requerido!")]
        public string Paterno { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo de apellido Materno es requerido!")]
        public string Materno { get; set; }
        [Required(ErrorMessage = "¡El campo de Fecha de Nacimiento es requerido!")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaNacimiento { get; set; }
        /*[StringLength(50)]
        [Required(ErrorMessage = "¡El campo Matricula es requerido!")]
        public string Matricula { get; set; } */
        [Range(0, float.MaxValue, ErrorMessage = "¡El campo Mensualidad debe ser mayor a 0 !")]
        [Required(ErrorMessage = "¡El campo Mensualidad es requerido!")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "¡El campo de Mensualidad solo debe contener números enteros o flotantes con un máximo de dos decimales!")]
        public float Mensualidad { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "¡El campo Modalidad debe ser mayor a 0 !")]
        [Required(ErrorMessage = "¡Selecciona una Modalidad!")]
        public int Modalidad { get; set; }
        [Required(ErrorMessage = "¡Selecciona un género!")]
        public string Genero { get; set; }
        [Required(ErrorMessage = "¡Selecciona un grado!")]
        public int GradoId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "¡El campo Beca debe ser mayor a 0 !")]
        [RegularExpression(@"^\d+$", ErrorMessage = "¡El campo de Beca solo debe contener números enteros!")]
        public int? Beca { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo Nombre del Tutor es requerido!")]
        public string NombreTutor { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo de apellido Paterno del tutor es requerido!")]
        public string PaternoTutor { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo de apellido Materno del tutor es requerido!")]
        public string MaternoTutor { get; set; }
        [StringLength(10)]
        [RegularExpression(@"^\d+$", ErrorMessage = "¡El campo de Teléfono del tutor solo debe contener números enteros!")]
        [Required(ErrorMessage = "¡El campo de Télefono del tutor es requerido!")]
        public string TelefonoTutor { get; set; }
        [Required(ErrorMessage = "¡El campo de Correo electrónico del tutor es requerido!")]
        [EmailAddress(ErrorMessage = "¡La dirección de Correo electrónico no es válida!")]
        public string CorreoElectronico { get; set; }
        [Required(ErrorMessage = "¡Selecciona un Género al tutor!")]
        public string GeneroTutor { get; set; }
    }
}
