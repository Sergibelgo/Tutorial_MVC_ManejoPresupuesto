using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class TransaccionDTO:Transaccion
    {
        public IEnumerable<SelectListItem> Cuentas { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }
        public Cuenta cuenta { get; set; }
        public Categoria categoria { get; set; }
        public string NombreCategoria { get; set; }
        public int CuentaAnteriorId { get; set; }
        public decimal MontoAnterior { get; set; }
    }
}
