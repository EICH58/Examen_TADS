namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Semestre
    {
        [Key]
        public int Id_Semestre { get; set; }

        [Required]
        public DateTime Fecha_Inicio { get; set; }

        [Required]
        public DateTime Fecha_Final { get; set; }

        [Required]
        public int Num_Semestre { get; set; }

        public ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();
    }
}
