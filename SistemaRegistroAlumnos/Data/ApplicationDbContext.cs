using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Models;

namespace SistemaRegistroAlumnos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // ==================== Tablas principales ====================
        public DbSet<Semestre> Semestre { get; set; }
        public DbSet<StatusAlumno> StatusAlumno { get; set; }
        public DbSet<Carrera> Carrera { get; set; }
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<PerfilSocioeconomico> PerfilesSocioeconomico { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Unidad> Unidades { get; set; }

        // ==================== Tablas académicas ====================
        public DbSet<Asistencia> Asistencia { get; set; }
        public DbSet<EstadoAsistencia> EstadoAsistencia { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }

        // ==================== Configuración del modelo ====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nombres de tablas explícitos (evita errores con EF pluralizador)
            modelBuilder.Entity<Alumno>().ToTable("Alumno");
            modelBuilder.Entity<Carrera>().ToTable("Carrera");
            modelBuilder.Entity<Semestre>().ToTable("Semestre");
            modelBuilder.Entity<StatusAlumno>().ToTable("Status_Alumno");
            modelBuilder.Entity<Asistencia>().ToTable("Asistencia");
            modelBuilder.Entity<Calificacion>().ToTable("Calificaciones");
            modelBuilder.Entity<EstadoAsistencia>().ToTable("Estado_Asistencia");

            // ==================== Índices útiles ====================
            // Los ID ya están indexados automáticamente (por ser clave primaria)
            // Así que aquí solo se crean índices adicionales de utilidad real

            modelBuilder.Entity<Alumno>()
                .HasIndex(a => a.Num_Control)
                .IsUnique();

            modelBuilder.Entity<Alumno>()
                .HasIndex(a => new { a.Nom_Alumno, a.App_Alumno });

            modelBuilder.Entity<Calificacion>()
                .HasIndex(c => c.Id_Alumno_Calif);

            // ==================== Relaciones uno a uno ====================
            modelBuilder.Entity<Alumno>()
                .HasOne(a => a.PerfilSocioeconomico)
                .WithOne(p => p.Alumno)
                .HasForeignKey<PerfilSocioeconomico>(p => p.Id_Alumno);

            // ==================== Tipos de columna ====================
            modelBuilder.Entity<Alumno>()
                .Property(a => a.Fecha_Nac)
                .HasColumnType("date");
        }
    }
}
