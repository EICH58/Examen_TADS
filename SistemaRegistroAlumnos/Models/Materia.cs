namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Materia
    {
        [Key]
        public int Id_Materia { get; set; }

        [Required]
        [StringLength(15)]
        public string Clave_Materia { get; set; }

        [Required]
        [StringLength(15)]
        public string Nombre_Materia { get; set; }

        [Required]
        public int Creditos_Materia { get; set; }

        [ForeignKey("Carrera")]
        public int Id_Carrera_Materia { get; set; }

        public virtual Carrera Carrera { get; set; }
        public virtual ICollection<Unidad> Unidades { get; set; }
    }
}
