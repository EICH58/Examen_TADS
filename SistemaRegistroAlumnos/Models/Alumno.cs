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

        [Required(ErrorMessage = "El número de control es requerido")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El número de control debe tener exactamente 8 dígitos")]
        [Column("num_control")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El número de control debe contener solo 8 dígitos numéricos")]
        public required string Num_Control { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+(\s[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)?$", ErrorMessage = "El nombre debe empezar con mayúscula, tener al menos 2 letras y si tiene dos nombres deben estar separados por espacio")]
        [Column("nom_alumno")]
        public required string Nom_Alumno { get; set; }


        [Required(ErrorMessage = "El apellido paterno es requerido")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ]{1,}$", ErrorMessage = "El apellido paterno debe empezar con mayúscula y tener al menos 2 letras")]
        [Column("app_paterno")]
        public string? App_Alumno { get; set; }

       // [Required(ErrorMessage = "El apellido materno es requerido")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ]{1,}$", ErrorMessage = "El apellido materno debe empezar con mayúscula y tener al menos 2 letras")]
        [Column("app_materno")]
        public string? Apm_Alumno { get; set; }

        [Required]
        [StringLength(1)]
        [Column("gen_alumno")]
        public required string Gen_Alumno { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [CustomValidation(typeof(Alumno), nameof(ValidateFechaNacimiento))]
        [Column("fecha_nac")]
        public DateOnly Fecha_Nac { get; set; }

        [ForeignKey("Carrera")]
        [Column("Id_Carrera_alum")]
        public int Id_Carrera_Alum { get; set; }

        [Required(ErrorMessage = "El número de semestre es requerido")]
        [ForeignKey("Semestre")]
        [Column("Id_Semestre_alum")]
        public int Id_Semestre_Alum { get; set; }

        [Required(ErrorMessage = "El estatus del alumno es requerido")]
        [ForeignKey("StatusAlumno")]
        [Column("Id_Status_alum")]
        public int Id_Status_Alum { get; set; }

        [RegularExpression(@"^\d{5}$", ErrorMessage = "El código postal debe tener exactamente 5 dígitos")]
        [Column("cod_postal")]
        public int? Cod_Postal { get; set; }

        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]{1,}$", ErrorMessage = "La colonia debe empezar con mayúscula y tener al menos 2 caracteres")]
        [StringLength(30)]
        [Column("colonia_alum")]
        public string? Colonia_Alum { get; set; }

        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]{1,}$", ErrorMessage = "La calle debe empezar con mayúscula y tener al menos 2 caracteres")]
        [StringLength(30)]
        [Column("calle_alum")]
        public string? Calle_Alum { get; set; }

        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "El número interior no puede ser 0")]
        [Column("num_casa")]
        public int? Num_Casa { get; set; }

        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "El número exterior no puede ser 0")]
        [Column("num_ext")]
        public int? Num_Ext { get; set; }

        // Método de validación personalizado para la fecha de nacimiento
        public static ValidationResult ValidateFechaNacimiento(DateOnly fechaNac, ValidationContext context)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var edadMinima = hoy.AddYears(-17);
            var edadMaxima = hoy.AddYears(-100);

            if (fechaNac > edadMinima)
                return new ValidationResult("El alumno debe tener al menos 17 años");

            if (fechaNac < edadMaxima)
                return new ValidationResult("La edad no puede ser mayor a 100 años");

            return ValidationResult.Success;
        }
        // Relaciones
        public virtual Carrera? Carrera { get; set; }
        public virtual Semestre? Semestre { get; set; }
        public virtual StatusAlumno? StatusAlumno { get; set; }
        public virtual FactorPorAlumno? PerfilSocioeconomico { get; set; }
        public virtual ICollection<Asistencia>? Asistencias { get; set; }
        public virtual ICollection<Calificacion>? Calificaciones { get; set; }
    }
}
