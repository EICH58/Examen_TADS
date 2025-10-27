namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Asistencia
    {
        [Key]
        public int Id_Asistencia { get; set; }

        [ForeignKey("EstadoAsistencia")]
        public int Id_EstadoAsistencia_Asia { get; set; }

        [ForeignKey("Alumno")]
        public int Id_Alumno_Asia { get; set; }

        [ForeignKey("Unidad")]
        public int Id_Unidad_Asia { get; set; }

        [Required]
        public DateTime Fecha_Asia { get; set; }

        public virtual EstadoAsistencia EstadoAsistencia { get; set; }
        public virtual Alumno Alumno { get; set; }
        public virtual Unidad Unidad { get; set; }
    }
}
