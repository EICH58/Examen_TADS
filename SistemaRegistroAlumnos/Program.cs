<<<<<<< Updated upstream
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
=======
using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // <-- Agregado

var builder = WebApplication.CreateBuilder(args);

// Configurar Entity Framework con MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    )
);

// Add services to the container
>>>>>>> Stashed changes
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ? Bloque de prueba simplificado (solo conexión)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (context.Database.CanConnect())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("? Conexión a la base de datos exitosa.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("? No se pudo conectar a la base de datos.");
            Console.ResetColor();
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("?? Error al probar la conexión: " + ex.Message);
        Console.ResetColor();
    }
}

app.Run();
