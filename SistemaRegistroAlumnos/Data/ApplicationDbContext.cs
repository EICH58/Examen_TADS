using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using SistemaRegistroAlumnos.Models;

namespace SistemaRegistroAlumnos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets originales
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

        // NUEVO: Usuarios
        public DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones originales
            modelBuilder.Entity<Alumno>()
                .HasIndex(a => a.Num_Control)
                .IsUnique();

            modelBuilder.Entity<Alumno>()
                .HasIndex(a => new { a.Nom_Alumno, a.App_Alumno });

            modelBuilder.Entity<Asistencia>()
                .HasIndex(a => a.Fecha_Asis);

            modelBuilder.Entity<Calificacion>()
                .HasIndex(c => c.Id_Alumno_Calif);

            modelBuilder.Entity<Alumno>()
                .HasOne(a => a.PerfilSocioeconomico)
                .WithOne(p => p.Alumno)
                .HasForeignKey<PerfilSocioeconomico>(p => p.Id_Alumno);

            modelBuilder.Entity<Alumno>()
                .Property(a => a.Fecha_Nac)
                .HasColumnType("date");

            // NUEVO: Configuración de Usuarios (copiada del scaffold)
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

                entity.Property(e => e.IdUsuario)
                    .HasColumnType("int(11)")
                    .HasColumnName("Id_Usuario");
                entity.Property(e => e.ContraUsuario)
                    .HasMaxLength(30)
                    .HasColumnName("contra_usuario");
                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(25)
                    .HasColumnName("nombre_usuario");
            });
        }
    }
}
