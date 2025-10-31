using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Data; // ?? importa tu contexto

namespace SistemaRegistroAlumnos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ?? Soluci�n al error de codificaci�n 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = WebApplication.CreateBuilder(args);

            // ======== Configuraci�n de servicios ========
            builder.Services.AddControllersWithViews();

            // ? Registro de tu DbContext (vuelve a agregar esta l�nea)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 36))
                )
            );

            var app = builder.Build();

            // ======== Configuraci�n del pipeline ========
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
