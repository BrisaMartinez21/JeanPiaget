using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JeanPiaget.Web.Models.Pagos
{
    public class PagoVM
    {
        public List<SelectListItem>? Grados { get; set; }
        public int? GradoId { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Nivel { get; set;}
        public int Fase { get; set; }
        public int Beca { get; set; }
        public float Mensualidad { get; set; }
        public float TotalPagar { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "¡El campo Monto debe ser mayor a 0 !")]
        [Required(ErrorMessage = "¡El campo Monto es requerido!")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "¡El campo de Monto solo debe contener números enteros o flotantes con un máximo de dos decimales!")]
        public float Monto { get; set; }
        [StringLength(20)]
        [Required(ErrorMessage = "¡El campo Folio es requerido!")]
        public string Folio { get; set; }
        [Required(ErrorMessage = "¡Selecciona un Concepto!")]
        public int Concepto { get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaActual { get; set; }
    }
}
