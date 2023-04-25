namespace Tutorial2ManejoPresupuesto.Models
{
    public class ReporteMensualDTO
    {
        public IEnumerable<ResultadoObtenerPorMes> TransaccionesPorMes { get; set; }
        public decimal Ingresos => TransaccionesPorMes.Sum(x => x.Ingreso);
        public decimal Gastos => TransaccionesPorMes.Sum(_ => _.Gasto);
        public decimal Total => Ingresos-Gastos;
        public int Año { get; set; }
    }
}
