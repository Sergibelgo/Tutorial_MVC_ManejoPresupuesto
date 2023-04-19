using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class TransaccionDTO:Transaccion
    {
        public IEnumerable<SelectListItem> Cuentas { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }
    }
}
