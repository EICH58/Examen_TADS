using Microsoft.AspNetCore.Http;
using SistemaRegistroAlumnos.Data;
using SistemaRegistroAlumnos.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace SistemaRegistroAlumnos.Includes
{
    public class BitacoraService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public BitacoraService(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public void RegistrarAccion(string accion, string? descripcion = null)
        {
            try
            {
                var userIdClaim = _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                    return;

                var ip = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Desconocida";

                var registro = new Bitacora
                {
                    Id_Usuario = userId,
                    Accion = accion,
                    Descripcion = descripcion,
                    Fecha_Hora = DateTime.Now,
                    IP_Address = ip
                };

                _context.Bitacora.Add(registro);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar en bitácora: {ex.Message}");
            }
        }
    }
}