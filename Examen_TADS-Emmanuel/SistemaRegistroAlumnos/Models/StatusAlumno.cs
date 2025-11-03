using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRegistroAlumnos.Models
{
    public class StatusAlumno
    {
        [Key]
        public int Id_Status { get; set; }

        [Required]
        [Column("estado_status")]
        [StringLength(20)]
        public string Estado_Status { get; set; } = string.Empty; // 👈 requerido, no nulo

        [Column("razon_status")]
        [StringLength(50)]
        public string? Razon_Status { get; set; } // 👈 puede ser nula

       // [Column("Id_Alumno_statusalum")]
       // public int Id_Alumno_statusalum { get; set; }

        public ICollection<Alumno>? Alumnos { get; set; }
    }
}
