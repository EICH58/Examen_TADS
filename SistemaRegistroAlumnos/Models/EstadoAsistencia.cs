using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRegistroAlumnos.Models
{
    [Table("estado_asistencia")] // 👈 nombre exacto de la tabla
    public class EstadoAsistencia
    {
        [Key]
        [Column("Id_EstadoAsistencia")]
        public int Id_EstadoAsistencia { get; set; }

        [Required]
        [StringLength(15)]
        [Column("estado_asistencia")]
        public string Estado_Asistencia { get; set; } = string.Empty;

        [StringLength(50)]
        [Column("razon_asistencia")]
        public string Razon_Asistencia { get; set; } = string.Empty;

        public virtual ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
    }
}
