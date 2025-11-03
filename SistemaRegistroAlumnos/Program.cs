using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SistemaRegistroAlumnos.Data;
using SistemaRegistroAlumnos.Includes; // ? Importa el BitacoraService que dejaste en Includes
using System.Text;

namespace SistemaRegistroAlumnos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ? Solución al error de codificación 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = WebApplication.CreateBuilder(args);

            // ======== Configuración de servicios ========
            builder.Services.AddControllersWithViews();

            // ? Registro del DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 36))
                )
            );

            // ? Registro del servicio de Bitácora
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<BitacoraService>();

            // ? Autenticación con cookies
            builder.Services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Denied";
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                });

            // ? Autorización para usar [Authorize]
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // ======== Configuración del pipeline ========
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}"
            );

            app.Run();
        }
    }
}
