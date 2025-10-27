using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRegistroAlumnos.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    Id_Carrera = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cod_Carrera = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Nombre_Carrera = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Creditos_Carrera = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.Id_Carrera);
                });

            migrationBuilder.CreateTable(
                name: "EstadosAsistencia",
                columns: table => new
                {
                    Id_EstadoAsistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado_Asistencia = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Razon_Inasistencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosAsistencia", x => x.Id_EstadoAsistencia);
                });

            migrationBuilder.CreateTable(
                name: "Semestres",
                columns: table => new
                {
                    Id_Semestre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha_Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fecha_Final = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Num_Semestre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semestres", x => x.Id_Semestre);
                });

            migrationBuilder.CreateTable(
                name: "StatusAlumnos",
                columns: table => new
                {
                    Id_Status = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado_Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Razon_Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusAlumnos", x => x.Id_Status);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id_Materia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clave_Materia = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Nombre_Materia = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Creditos_Materia = table.Column<int>(type: "int", nullable: false),
                    Id_Carrera_Materia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id_Materia);
                    table.ForeignKey(
                        name: "FK_Materias_Carreras_Id_Carrera_Materia",
                        column: x => x.Id_Carrera_Materia,
                        principalTable: "Carreras",
                        principalColumn: "Id_Carrera",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alumnos",
                columns: table => new
                {
                    Id_Alumno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Num_Control = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Nom_Alumno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    App_Alumno = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Apm_Alumno = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Gen_Alumno = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Fecha_Nac = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_Carrera_Alum = table.Column<int>(type: "int", nullable: false),
                    Id_Semestre_Alum = table.Column<int>(type: "int", nullable: false),
                    Id_Status_Alum = table.Column<int>(type: "int", nullable: false),
                    Cod_Programa = table.Column<int>(type: "int", nullable: false),
                    Colonia_Alum = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Calle_Alum = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Num_Casa = table.Column<int>(type: "int", nullable: false),
                    Num_Ext = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnos", x => x.Id_Alumno);
                    table.ForeignKey(
                        name: "FK_Alumnos_Carreras_Id_Carrera_Alum",
                        column: x => x.Id_Carrera_Alum,
                        principalTable: "Carreras",
                        principalColumn: "Id_Carrera",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumnos_Semestres_Id_Semestre_Alum",
                        column: x => x.Id_Semestre_Alum,
                        principalTable: "Semestres",
                        principalColumn: "Id_Semestre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumnos_StatusAlumnos_Id_Status_Alum",
                        column: x => x.Id_Status_Alum,
                        principalTable: "StatusAlumnos",
                        principalColumn: "Id_Status",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    Id_Unidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Unidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id_Materia_Unidad = table.Column<int>(type: "int", nullable: false),
                    Horas_Clase = table.Column<int>(type: "int", nullable: false),
                    Creditos_Unidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.Id_Unidad);
                    table.ForeignKey(
                        name: "FK_Unidades_Materias_Id_Materia_Unidad",
                        column: x => x.Id_Materia_Unidad,
                        principalTable: "Materias",
                        principalColumn: "Id_Materia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfilesSocioeconomicos",
                columns: table => new
                {
                    Id_Perfil_SE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Alumno = table.Column<int>(type: "int", nullable: false),
                    Ingreso_Familiar = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Personas_Hogar = table.Column<int>(type: "int", nullable: true),
                    Vivienda_Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilesSocioeconomicos", x => x.Id_Perfil_SE);
                    table.ForeignKey(
                        name: "FK_PerfilesSocioeconomicos_Alumnos_Id_Alumno",
                        column: x => x.Id_Alumno,
                        principalTable: "Alumnos",
                        principalColumn: "Id_Alumno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asistencias",
                columns: table => new
                {
                    Id_Asistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_EstadoAsistencia_Asia = table.Column<int>(type: "int", nullable: false),
                    Id_Alumno_Asia = table.Column<int>(type: "int", nullable: false),
                    Id_Unidad_Asia = table.Column<int>(type: "int", nullable: false),
                    Fecha_Asia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencias", x => x.Id_Asistencia);
                    table.ForeignKey(
                        name: "FK_Asistencias_Alumnos_Id_Alumno_Asia",
                        column: x => x.Id_Alumno_Asia,
                        principalTable: "Alumnos",
                        principalColumn: "Id_Alumno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asistencias_EstadosAsistencia_Id_EstadoAsistencia_Asia",
                        column: x => x.Id_EstadoAsistencia_Asia,
                        principalTable: "EstadosAsistencia",
                        principalColumn: "Id_EstadoAsistencia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asistencias_Unidades_Id_Unidad_Asia",
                        column: x => x.Id_Unidad_Asia,
                        principalTable: "Unidades",
                        principalColumn: "Id_Unidad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calificaciones",
                columns: table => new
                {
                    Id_Calificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calif_Final = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    Id_Alumno_Calif = table.Column<int>(type: "int", nullable: false),
                    Id_Unidad_Calif = table.Column<int>(type: "int", nullable: false),
                    Fecha_Registro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calificaciones", x => x.Id_Calificacion);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Alumnos_Id_Alumno_Calif",
                        column: x => x.Id_Alumno_Calif,
                        principalTable: "Alumnos",
                        principalColumn: "Id_Alumno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calificaciones_Unidades_Id_Unidad_Calif",
                        column: x => x.Id_Unidad_Calif,
                        principalTable: "Unidades",
                        principalColumn: "Id_Unidad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Id_Carrera_Alum",
                table: "Alumnos",
                column: "Id_Carrera_Alum");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Id_Semestre_Alum",
                table: "Alumnos",
                column: "Id_Semestre_Alum");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Id_Status_Alum",
                table: "Alumnos",
                column: "Id_Status_Alum");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Nom_Alumno_App_Alumno",
                table: "Alumnos",
                columns: new[] { "Nom_Alumno", "App_Alumno" });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_Num_Control",
                table: "Alumnos",
                column: "Num_Control",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Fecha_Asia",
                table: "Asistencias",
                column: "Fecha_Asia");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Id_Alumno_Asia",
                table: "Asistencias",
                column: "Id_Alumno_Asia");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Id_EstadoAsistencia_Asia",
                table: "Asistencias",
                column: "Id_EstadoAsistencia_Asia");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_Id_Unidad_Asia",
                table: "Asistencias",
                column: "Id_Unidad_Asia");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_Id_Alumno_Calif",
                table: "Calificaciones",
                column: "Id_Alumno_Calif");

            migrationBuilder.CreateIndex(
                name: "IX_Calificaciones_Id_Unidad_Calif",
                table: "Calificaciones",
                column: "Id_Unidad_Calif");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Id_Carrera_Materia",
                table: "Materias",
                column: "Id_Carrera_Materia");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilesSocioeconomicos_Id_Alumno",
                table: "PerfilesSocioeconomicos",
                column: "Id_Alumno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_Id_Materia_Unidad",
                table: "Unidades",
                column: "Id_Materia_Unidad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistencias");

            migrationBuilder.DropTable(
                name: "Calificaciones");

            migrationBuilder.DropTable(
                name: "PerfilesSocioeconomicos");

            migrationBuilder.DropTable(
                name: "EstadosAsistencia");

            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropTable(
                name: "Alumnos");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Semestres");

            migrationBuilder.DropTable(
                name: "StatusAlumnos");

            migrationBuilder.DropTable(
                name: "Carreras");
        }
    }
}
