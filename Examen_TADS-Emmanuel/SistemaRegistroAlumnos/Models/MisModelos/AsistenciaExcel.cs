namespace SistemaRegistroAlumnos.Models.MisModelos
{
    public class AsistenciaExcel
    {
        public int IdEstadoAsistencia { get; set; }
        public int IdAlumno { get; set; }
        public int IdUnidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
