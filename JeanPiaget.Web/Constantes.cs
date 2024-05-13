using JeanPiaget.Core.Materias;

namespace JeanPiaget.Web
{
    public static class Constantes
    {
        public static readonly string[] Roles = { "Root", "Administrador", "Direccion" };
        public static readonly Dictionary<string, string> UsuarioRoot = new Dictionary<string, string>
        {
            { "Nombre", "Root" },
            { "Cargo", "Master" },
            { "UserName", "root@jeanpiaget.com" },
            { "Email", "root@jeanpiaget.com" },
            { "PhoneNumber", "1234567890" },
            { "Password", "Pa$$word.1" },
        };

        public static readonly Grado[] Grados = new Grado[]
        {
            new Grado() { Nivel = "Kinder", Fase = 1 },
            new Grado() { Nivel = "Kinder", Fase = 2 },
            new Grado() { Nivel = "Kinder", Fase = 3 },
            new Grado() { Nivel = "Primaria", Fase = 1 },
            new Grado() { Nivel = "Primaria", Fase = 2 },
            new Grado() { Nivel = "Primaria", Fase = 3 },
            new Grado() { Nivel = "Primaria", Fase = 4 },
            new Grado() { Nivel = "Primaria", Fase = 5 },
            new Grado() { Nivel = "Primaria", Fase = 6 },
            new Grado() { Nivel = "Secundaria", Fase = 1 },
            new Grado() { Nivel = "Secundaria", Fase = 2 },
            new Grado() { Nivel = "Secundaria", Fase = 3 },
            new Grado() { Nivel = "Preparatoria", Fase = 1 },
            new Grado() { Nivel = "Preparatoria", Fase = 2 },
            new Grado() { Nivel = "Preparatoria", Fase = 3 },
        };
    }
}
