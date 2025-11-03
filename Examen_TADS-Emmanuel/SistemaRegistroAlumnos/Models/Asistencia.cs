using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRegistroAlumnos.Models
{
    [Table("asistencia")] // 👈 asegura coincidencia exacta con la tabla
    public class Asistencia
    {
        [Key]
        [Column("Id_Asistencia")]
        public int Id_Asistencia { get; set; }

        [ForeignKey("EstadoAsistencia")]
        [Column("Id_EstadoAsis_asis")] // 👈 coincide con la BD
        public int Id_EstadoAsis_Asis { get; set; }

        [ForeignKey("Alumno")]
        [Column("Id_Alumno_asis")]
        public int Id_Alumno_Asis { get; set; }

        [ForeignKey("Unidad")]
        [Column("Id_Unidad_asis")]
        public int Id_Unidad_Asis { get; set; }

        [Required]
        [Column("fecha_asis")]
        public DateTime Fecha_Asis { get; set; }

        // Relaciones de navegación
        public virtual EstadoAsistencia EstadoAsistencia { get; set; } = null!;
        public virtual Alumno Alumno { get; set; } = null!;
        public virtual Unidad Unidad { get; set; } = null!;
    }
}
