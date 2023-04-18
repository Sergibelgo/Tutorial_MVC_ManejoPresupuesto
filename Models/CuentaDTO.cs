using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class CuentaDTO:Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
