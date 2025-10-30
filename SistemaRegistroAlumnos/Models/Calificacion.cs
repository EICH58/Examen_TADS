using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRegistroAlumnos.Models
{
    public class Calificacion
    {
        [Key]
        public int Id_Calificacion { get; set; }

        [Required]
        [Column("calif_indiv", TypeName = "decimal(5,2)")]
        public decimal Calif_Indiv { get; set; }

        [ForeignKey("Alumno")]
        public int Id_Alumno_Calif { get; set; }

        [ForeignKey("Unidad")]
        public int Id_Unidad_Calif { get; set; }

        [Required]
        public DateTime? Fecha_Registro { get; set; }

        // Relaciones
        public virtual Alumno Alumno { get; set; } = null!;
        public virtual Unidad Unidad { get; set; } = null!;
    }
}
