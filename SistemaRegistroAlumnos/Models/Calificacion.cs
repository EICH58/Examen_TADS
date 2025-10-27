namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Calificacion
    {
        [Key]
        public int Id_Calificacion { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,2)")]
        public decimal Calif_Final { get; set; }

        [ForeignKey("Alumno")]
        public int Id_Alumno_Calif { get; set; }

        [ForeignKey("Unidad")]
        public int Id_Unidad_Calif { get; set; }

        [Required]
        public DateTime Fecha_Registro { get; set; }

        public virtual Alumno Alumno { get; set; }
        public virtual Unidad Unidad { get; set; }
    }
}
