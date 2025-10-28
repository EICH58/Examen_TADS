namespace SistemaRegistroAlumnos.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Alumno")]
    public class Alumno
    {
        [Key]
        [Column("Id_Alumno")]
        public int Id_Alumno { get; set; }

        [Required]
        [StringLength(15)]
        [Column("num_control")]
        public required string Num_Control { get; set; }

        [Required]
        [StringLength(50)]
        [Column("nom_alumno")]
        public required string Nom_Alumno { get; set; }

        [StringLength(25)]
        [Column("app_paterno")]
        public string? App_Alumno { get; set; }

        [StringLength(25)]
        [Column("app_materno")]
        public string? Apm_Alumno { get; set; }

        [Required]
        [StringLength(1)]
        [Column("gen_alumno")]
        public required string Gen_Alumno { get; set; }

        [Required]
        [Column("fecha_nac")]
        public DateOnly Fecha_Nac { get; set; }

        [ForeignKey("Carrera")]
        [Column("Id_Carrera_alum")]
        public int Id_Carrera_Alum { get; set; }

        [ForeignKey("Semestre")]
        [Column("Id_Semestre_alum")]
        public int Id_Semestre_Alum { get; set; }

        [ForeignKey("StatusAlumno")]
        [Column("Id_Status_alum")]
        public int Id_Status_Alum { get; set; }

        [Column("cod_postal")]
        public int? Cod_Postal { get; set; }

        [StringLength(30)]
        [Column("colonia_alum")]
        public string? Colonia_Alum { get; set; }

        [StringLength(30)]
        [Column("calle_alum")]
        public string? Calle_Alum { get; set; }

        [Column("num_casa")]
        public int? Num_Casa { get; set; }

        [Column("num_ext")]
        public int? Num_Ext { get; set; }

        // Relaciones
        public virtual Carrera? Carrera { get; set; }
        public virtual Semestre? Semestre { get; set; }
        public virtual StatusAlumno? StatusAlumno { get; set; }
        public virtual PerfilSocioeconomico? PerfilSocioeconomico { get; set; }
        public virtual ICollection<Asistencia>? Asistencias { get; set; }
        public virtual ICollection<Calificacion>? Calificaciones { get; set; }
    }
}
