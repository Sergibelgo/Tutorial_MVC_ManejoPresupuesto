namespace Tutorial2ManejoPresupuesto.Models
{
    public class TransaccionesViewDTO
    {
        public IEnumerable<TransaccionDTO> transacciones;
        public decimal Ingresos => transacciones.Where(x => x.TipoOperacionId == (int)TipoOperacion.Ingreso).Sum(x => x.Monto);
        public decimal Gastos => transacciones.Where(x => x.TipoOperacionId == (int)TipoOperacion.Gasto).Sum(x => x.Monto);
        public decimal Total => Ingresos-Gastos;
    }
}
