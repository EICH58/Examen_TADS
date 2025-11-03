using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRegistroAlumnos.Models
{
    public class Bitacora
    {
        [Key]
        public int Id_Bitacora { get; set; }

        [ForeignKey("Usuario")]
        public int Id_Usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Accion { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime Fecha_Hora { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? IP_Address { get; set; }

        // Navegación
        public virtual Usuarios Usuario { get; set; } = null!;
    }
}