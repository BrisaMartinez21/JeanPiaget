using JeanPiaget.Core.Alumnos;
using JeanPiaget.Core.Calificaciones;
using JeanPiaget.Core.Materias;
using JeanPiaget.Core.Pagos;
using JeanPiaget.Core.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanPiaget.DataAccess
{
    public class JeanPiagetContext : IdentityDbContext
    {
        public virtual DbSet<Alumno> Alumnos { get; set; }
        // public virtual DbSet<Beca> Becas { get; set; }
        public virtual DbSet<Pago> Pagos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Tutor> Tutores { get; set; }
        public virtual DbSet<Calificacion> Calificaciones { get; set; }
        public virtual DbSet<Grado> Grados { get; set; }
        public virtual DbSet<Materia> Materias { get; set; }

        //Constructores del contexto 
        public JeanPiagetContext() { }  
        public JeanPiagetContext(DbContextOptions options) : base (options) { }
    }
}
