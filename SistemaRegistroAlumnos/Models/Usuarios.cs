using System;
using System.Collections.Generic;

namespace SistemaRegistroAlumnos.Models;

public partial class Usuarios
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ContraUsuario { get; set; } = null!;
}
