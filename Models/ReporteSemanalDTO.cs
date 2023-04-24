namespace Tutorial2ManejoPresupuesto.Models
{
    public class ReporteSemanalDTO
    {
        public decimal Ingresos =>TransaccionesPorSemana.Sum(x=>x.Ingresos);
        public decimal Gastos => TransaccionesPorSemana.Sum(x => x.Gastos);
        public DateTime FechaReferencia { get; set; }
        public IEnumerable<ResultadoObtenerPorSemana> TransaccionesPorSemana { get; set; }
    }
}
