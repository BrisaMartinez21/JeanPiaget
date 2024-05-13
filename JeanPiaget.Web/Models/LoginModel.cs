using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JeanPiaget.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "¡El campo Email/UserName es requerido!")]

        public string Email { get; set; }

        [Required(ErrorMessage = "¡El campo Password es requerido!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
