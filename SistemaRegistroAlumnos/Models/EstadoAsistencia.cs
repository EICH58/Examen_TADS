namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EstadoAsistencia
    {
        [Key]
        public int Id_EstadoAsistencia { get; set; }

        [Required]
        [StringLength(15)]
        public string Estado_Asistencia { get; set; }

        [StringLength(50)]
        public string Razon_Inasistencia { get; set; }

        public virtual ICollection<Asistencia> Asistencias { get; set; }
    }
}
