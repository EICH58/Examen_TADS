namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Unidad
    {
        [Key]
        public int Id_Unidad { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre_Unidad { get; set; }

        [ForeignKey("Materia")]
        public int Id_Materia_Unidad { get; set; }

        [Required]
        public int Horas_Clase { get; set; }

        [Required]
        public int Creditos_Unidad { get; set; }

        public virtual Materia Materia { get; set; }
        public virtual ICollection<Asistencia> Asistencias { get; set; }
        public virtual ICollection<Calificacion> Calificaciones { get; set; }
    }
}
