using JeanPiaget.Core.DTOs.Usuarios;
using JeanPiaget.Core.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.ApplicationServices.Usuarios
{
    public interface IUsuarioAppService
    {
        ValueTask<Usuario> GetUsuarioAsync(string usuarioId);

        Task<Usuario> GetUsuarioByNameAsync(string name);

        Task<Usuario> GetUsuarioByEmailAsync(string email);

        Task EditUsuarioAsync(Usuario usuario);

        Task<List<UsuarioRolDTO>> GetUsuarioFilterAsync(string busqueda, int filtro, int pagina, int cantidad, Dictionary<string, string> userIgnore);

        Task<int> TotaUsuarioFilterAsync(string busqueda, int filtro, Dictionary<string, string> userIgnore);

        Task<List<UsuarioRolDTO>> PaginationUsuario(int pagina, int cantidad, Dictionary<string, string> userIgnore);

        Task<int> TotalUsuarios(Dictionary<string, string> userIgnore);
    }
}
