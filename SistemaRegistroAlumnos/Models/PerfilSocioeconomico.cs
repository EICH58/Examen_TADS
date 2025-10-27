namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PerfilSocioeconomico
    {
        [Key]
        public int Id_Perfil_SE { get; set; }

        [ForeignKey("Alumno")]
        public int Id_Alumno { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Ingreso_Familiar { get; set; }

        public int? Personas_Hogar { get; set; }

        [StringLength(50)]
        public string Vivienda_Tipo { get; set; }

        public virtual Alumno Alumno { get; set; }
    }
}
