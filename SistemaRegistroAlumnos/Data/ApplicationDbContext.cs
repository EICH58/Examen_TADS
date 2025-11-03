using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Models;

namespace SistemaRegistroAlumnos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // ==================== DbSets ====================
        public DbSet<Semestre> Semestre { get; set; }
        public DbSet<StatusAlumno> StatusAlumno { get; set; }
        public DbSet<Carrera> Carrera { get; set; }
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<Asistencia> Asistencia { get; set; }
        public DbSet<EstadoAsistencia> EstadoAsistencia { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<FactorPorAlumno> FactoresPorAlumno { get; set; }  // ← AGREGAR ESTA LÍNEA
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Bitacora> Bitacora { get; set; }

        // ==================== Mapeo y configuración ====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Nombres de tabla EXACTOS ---
            modelBuilder.Entity<Alumno>().ToTable("Alumno");
            modelBuilder.Entity<Carrera>().ToTable("Carrera");
            modelBuilder.Entity<Semestre>().ToTable("Semestre");
            modelBuilder.Entity<StatusAlumno>().ToTable("Status_Alumno");
            modelBuilder.Entity<Asistencia>().ToTable("Asistencia");
            modelBuilder.Entity<Calificacion>().ToTable("Calificaciones");
            modelBuilder.Entity<EstadoAsistencia>().ToTable("Estado_Asistencia");
            modelBuilder.Entity<Materia>().ToTable("Materias");
            modelBuilder.Entity<Unidad>().ToTable("Unidades");
            modelBuilder.Entity<FactorPorAlumno>().ToTable("Factor_Por_Alumno");
            modelBuilder.Entity<Bitacora>().ToTable("Bitacora");

            // --- Índices útiles ---
            modelBuilder.Entity<Alumno>()
                .HasIndex(a => a.Num_Control)
                .IsUnique();

            modelBuilder.Entity<Alumno>()
                .HasIndex(a => new { a.Nom_Alumno, a.App_Alumno });

            modelBuilder.Entity<Asistencia>()
                .HasIndex(a => a.Fecha_Asis);

            modelBuilder.Entity<Calificacion>()
                .HasIndex(c => c.Id_Alumno_Calif);

            modelBuilder.Entity<FactorPorAlumno>()  
                .HasIndex(f => f.Id_Alumno_factor);

            // --- Tipos de columna específicos ---
            modelBuilder.Entity<Alumno>()
                .Property(a => a.Fecha_Nac)
                .HasColumnType("date");

            modelBuilder.Entity<Bitacora>()
                .HasIndex(b => b.Fecha_Hora);
            modelBuilder.Entity<Bitacora>()
                .HasIndex(b => b.Id_Usuario);

            // --- Usuarios (login) ---
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario)
                      .HasColumnName("Id_Usuario")
                      .HasColumnType("int(11)");
                entity.Property(e => e.NombreUsuario)
                      .HasColumnName("nombre_usuario")
                      .HasMaxLength(25);
                entity.Property(e => e.ContraUsuario)
                      .HasColumnName("contra_usuario")
                      .HasMaxLength(30);
            });
        }
    }
}