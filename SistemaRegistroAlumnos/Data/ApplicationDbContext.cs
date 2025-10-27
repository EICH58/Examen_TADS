using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SistemaRegistroAlumnos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Semestre> Semestre { get; set; }
        public DbSet<StatusAlumno> Status_Alumno { get; set; }
        public DbSet<Carrera> Carrera { get; set; }
        public DbSet<Alumno> Alumno { get; set; }
        public DbSet<PerfilSocioeconomico> Perfil_Socioeconomico { get; set; }
        public DbSet<EstadoAsistencia> EstadosAsistencia { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<Asistencia> Asistencia { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales
            modelBuilder.Entity<Alumno>()
                .HasIndex(a => a.Num_Control)
                .IsUnique();

            modelBuilder.Entity<Alumno>()
                .HasIndex(a => new { a.Nom_Alumno, a.App_Alumno });

            modelBuilder.Entity<Asistencia>()
                .HasIndex(a => a.Fecha_Asia);

            modelBuilder.Entity<Calificacion>()
                .HasIndex(c => c.Id_Alumno_Calif);

            // Relaciones uno a uno
            modelBuilder.Entity<Alumno>()
                .HasOne(a => a.PerfilSocioeconomico)
                .WithOne(p => p.Alumno)
                .HasForeignKey<PerfilSocioeconomico>(p => p.Id_Alumno);
           
            modelBuilder.Entity<Alumno>()
                .Property(a => a.Fecha_Nac)
                .HasColumnType("date");
        }
    }
}