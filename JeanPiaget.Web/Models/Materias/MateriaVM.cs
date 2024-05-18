using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JeanPiaget.Web.Models.Materias
{
    public class MateriaVM
    {
        public int? Id { get; set; }
        public List<SelectListItem>? Grados { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "¡El campo Nombre es requerido!")]
        public string Nombre { get; set; }

        /*[Range(0, int.MaxValue, ErrorMessage = "¡El campo Clave debe ser mayor a 0!")]
        [Required(ErrorMessage = "¡El campo Clave es requerido!")]
        [RegularExpression(@"^\d+$", ErrorMessage = "¡El campo de Clave solo debe contener números enteros!")]
        public int Clave { get; set; }*/


        [Required(ErrorMessage = "¡Selecciona un grado!")]
        public int GradoId { get; set; }
    }
}
