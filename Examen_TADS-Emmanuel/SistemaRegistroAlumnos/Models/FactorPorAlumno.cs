using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRegistroAlumnos.Models
{
    public class FactorPorAlumno
    {
        [Key]
        public int Id_FactorxAlum { get; set; }

        [ForeignKey("Alumno")]
        public int Id_Alumno_factor { get; set; }

        [Required]
        [StringLength(50)]
        public string Factor { get; set; } = null!;

        public int Severidad { get; set; }

        [StringLength(200)]
        public string? Descripcion { get; set; }

        // Navegación
        public virtual Alumno Alumno { get; set; } = null!;
    }
}