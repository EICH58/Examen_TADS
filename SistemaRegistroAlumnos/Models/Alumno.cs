namespace SistemaRegistroAlumnos.Models
{
    using SistemaRegistroAlumnos.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Alumno
    {
        [Key]
        public int Id_Alumno { get; set; }

        [Required]
        [StringLength(15)]
        public required string Num_Control { get; set; }

        [Required]
        [StringLength(50)]
        public required string Nom_Alumno { get; set; }

        [StringLength(25)]
        public required string App_Alumno { get; set; }

        [StringLength(25)]
        public required string Apm_Alumno { get; set; }

        [Required]
        [StringLength(1)]
        public required string Gen_Alumno { get; set; }

        [Required]
        public DateOnly Fecha_Nac { get; set; }

        [ForeignKey("Carrera")]
        public int Id_Carrera_Alum { get; set; }

        [ForeignKey("Semestre")]
        public int Id_Semestre_Alum { get; set; }

        [ForeignKey("StatusAlumno")]
        public int Id_Status_Alum { get; set; }

        public int Cod_Programa { get; set; }

        [Required]
        [StringLength(30)]
        public required string Colonia_Alum { get; set; }

        [Required]
        [StringLength(30)]
        public required string Calle_Alum { get; set; }

        [Required]
        public int Num_Casa { get; set; }

        public int? Num_Ext { get; set; }

        // Navigation properties
        public virtual Carrera? Carrera { get; set; }
        public virtual Semestre? Semestre { get; set; }
        public virtual StatusAlumno? StatusAlumno { get; set; }
        public virtual PerfilSocioeconomico? PerfilSocioeconomico { get; set; }
        public virtual ICollection<Asistencia>? Asistencias { get; set; }
        public virtual ICollection<Calificacion>? Calificaciones { get; set; }
    }
}