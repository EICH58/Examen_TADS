namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;

    public class StatusAlumno
    {
        [Key]
        public int Id_Status { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado_Status { get; set; }

        [StringLength(50)]
        public string Razon_Status { get; set; }

        public ICollection<Alumno> Alumnos { get; set; }
    }
}
