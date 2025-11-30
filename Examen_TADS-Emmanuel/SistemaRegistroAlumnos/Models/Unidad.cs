namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Unidades")] // 👈 nombre real de la tabla
    public class Unidad
    {
        [Key]
        [Column("Id_Unidades")] // 👈 nombre real del campo PK
        public int Id_Unidades { get; set; }

        [Required]
        [StringLength(50)]
        [Column("nombre_unidad")]
        public string Nombre_Unidad { get; set; } = null!;


        [ForeignKey("Materia")]
        [Column("Id_Materia_unidad")] // 👈 respeta el nombre en MySQL
        public int Id_Materia_Unidad { get; set; }

        [Required]
        [Column("dias_clase")] // 👈 coincide con tu BD
        public int Dias_Clase { get; set; }

        [Required]
        [Column("creditos_unidad")] // 👈 igual que BD
        public int Creditos_Unidad { get; set; }

        public virtual Materia Materia { get; set; } = null!;
        public virtual ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
        public virtual ICollection<Calificacion> Calificaciones { get; set; } = new List<Calificacion>();
    }
}
