namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Carrera
    {
        [Key]
        public int Id_Carrera { get; set; }

        [Required]
        [StringLength(15)]
        public string Cod_Carrera { get; set; }

        [Required]
        [StringLength(15)]
        public string Nombre_Carrera { get; set; }

        [Required]
        public int Creditos_Carrera { get; set; }

        public ICollection<Alumno> Alumnos { get; set; }
        public ICollection<Materia> Materias { get; set; }
    }
}
