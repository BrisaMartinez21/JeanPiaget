using JeanPiaget.Core.DTOs.Usuarios;

namespace JeanPiaget.Web.Models.User
{
    public class UserVMPaginacion : BaseModeloPaginacion
    {
        public List<UsuarioRolDTO> Usuarios { get; set; }
    }
}
