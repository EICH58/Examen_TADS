namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    [Table("Carrera")]
    public class Carrera
    {
        [Key]
        public int Id_Carrera { get; set; }

        [Required]
        [StringLength(15)]
        public required string Cod_Carrera { get; set; }

        [Required]
        [StringLength(15)]
        public required string Nombre_Carrera { get; set; }

        [Required]
        public int Creditos_Carrera { get; set; }

        public ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();
        public ICollection<Materia> Materias { get; set; } = new List<Materia>();
    }
}
