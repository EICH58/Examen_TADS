using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRegistroAlumnos.Models
{
    [Table("Usuarios")] // ✅ Nombre exacto de la tabla
    public class Usuarios
    {
        [Key]
        [Column("Id_Usuario")] // ✅ Coincide con la BD
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(25)]
        [Column("nombre_usuario")] // ✅ Coincide con la BD
        public string NombreUsuario { get; set; } = null!;

        [Required]
        [StringLength(30)]
        [Column("contra_usuario")] // ✅ Coincide con la BD
        public string ContraUsuario { get; set; } = null!;

        [Required]
        [StringLength(20)]
        [Column("Rol")] // ✅ (ya coincide, no hay guion bajo)
        public string Rol { get; set; } = "Maestro";

        [StringLength(100)]
        [Column("Nombre_Completo")] // ✅ Aquí estaba el problema principal
        public string? NombreCompleto { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Column("Correo")] // ✅ (ya coincide)
        public string? Correo { get; set; }
    }
}