using SistemaRegistroAlumnos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class AlumnoTests
{
    // Método auxiliar para validar un modelo con DataAnnotations
    private IList<ValidationResult> ValidateModel(object model)
    {
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, results, true);
        return results;
    }

    // Crea un Alumno válido para usar como plantilla en varias pruebas
    private Alumno CreateValidAlumno()
    {
        return new Alumno
        {
            Id_Alumno = 1,
            Num_Control = "12345678",
            Nom_Alumno = "Pedro",
            App_Alumno = "López",
            Apm_Alumno = "García",
            Gen_Alumno = "M",
            Fecha_Nac = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            Id_Carrera_Alum = 1,
            Id_Semestre_Alum = 1,
            Id_Status_Alum = 1,
            Cod_Postal = 12345,
            Colonia_Alum = "Colonia Centro",
            Calle_Alum = "Calle Uno",
            Num_Casa = 10,
            Num_Ext = 20
        };
    }

    // -----------------------------------------
    // PRUEBAS DE CAMPOS REQUERIDOS
    // -----------------------------------------

    [Fact]
    public void Alumno_NumControl_Requerido()
    {
        var alumno = CreateValidAlumno();
        alumno.Num_Control = null;

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.MemberNames.Contains("Num_Control"));
    }

    [Fact]
    public void Alumno_Nombre_Requerido()
    {
        var alumno = CreateValidAlumno();
        alumno.Nom_Alumno = null;

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.MemberNames.Contains("Nom_Alumno"));
    }

    [Fact]
    public void Alumno_Genero_Requerido()
    {
        var alumno = CreateValidAlumno();
        alumno.Gen_Alumno = null;

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.MemberNames.Contains("Gen_Alumno"));
    }

    // -----------------------------------------
    // PRUEBA EXPRESIONES REGULARES
    // -----------------------------------------

    [Fact]
    public void Alumno_NumControl_DebeSer8Digitos()
    {
        var alumno = CreateValidAlumno();
        alumno.Num_Control = "ABC123"; // inválido

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.MemberNames.Contains("Num_Control"));
    }

    [Fact]
    public void Alumno_Nombre_FormatoInvalido()
    {
        var alumno = CreateValidAlumno();
        alumno.Nom_Alumno = "pedro"; // debe iniciar con mayúscula

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.MemberNames.Contains("Nom_Alumno"));
    }

    [Fact]
    public void Alumno_ApellidoPaterno_FormatoInvalido()
    {
        var alumno = CreateValidAlumno();
        alumno.App_Alumno = "lopez"; // inválido

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.MemberNames.Contains("App_Alumno"));
    }

    // -----------------------------------------
    // VALIDACIÓN DE FECHA DE NACIMIENTO
    // -----------------------------------------

    [Fact]
    public void Alumno_DebeTenerMinimo17Anios()
    {
        var alumno = CreateValidAlumno();
        alumno.Fecha_Nac = DateOnly.FromDateTime(DateTime.Today.AddYears(-16));

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.ErrorMessage.Contains("al menos 17 años"));
    }

    [Fact]
    public void Alumno_NoDebeTenerMasDe100Anios()
    {
        var alumno = CreateValidAlumno();
        alumno.Fecha_Nac = DateOnly.FromDateTime(DateTime.Today.AddYears(-105));

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.ErrorMessage.Contains("mayor a 100 años"));
    }

    // -----------------------------------------
    // PRUEBAS DE CAMPOS OPCIONALES CON VALIDACIÓN
    // -----------------------------------------

    [Fact]
    public void Alumno_CodigoPostal_Invalido()
    {
        var alumno = CreateValidAlumno();
        alumno.Cod_Postal = 12; // inválido, deben ser 5 dígitos

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.MemberNames.Contains("Cod_Postal"));
    }

    [Fact]
    public void Alumno_Colonia_FormatoInvalido()
    {
        var alumno = CreateValidAlumno();
        alumno.Colonia_Alum = "colonia"; // inválido

        var results = ValidateModel(alumno);

        Assert.Contains(results, r => r.MemberNames.Contains("Colonia_Alum"));
    }
}