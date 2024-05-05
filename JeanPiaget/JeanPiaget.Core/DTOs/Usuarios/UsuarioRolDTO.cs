using JeanPiaget.Core.Usuarios;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.Core.DTOs.Usuarios
{
    public class UsuarioRolDTO
    {
        public Usuario Usuario { get; set; }
        public IdentityRole Role { get; set; }
    }
}
