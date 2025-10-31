namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    public class Materia
    {
        [Key]
        public int Id_Materia { get; set; }

        [Required]
        [StringLength(15)]
        public string Clave_Materia { get; set; } = null!;

        [Required]
        [StringLength(15)]
        public string Nombre_Materia { get; set; } = null!;

        [Required]
        public int Creditos_Materia { get; set; }

        [ForeignKey("Carrera")]
        public int Id_Carrera_Materia { get; set; }

        public virtual Carrera Carrera { get; set; } = null!;
        public virtual ICollection<Unidad> Unidades { get; set; } = new List<Unidad>();
    }
}
