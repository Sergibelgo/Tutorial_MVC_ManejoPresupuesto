using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        public TiposCuentasController(ITiposCuentasServices tiposCuentasServices)
        {
            _tiposCuentasServices = tiposCuentasServices;
        }

        public ITiposCuentasServices _tiposCuentasServices { get; }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            _tiposCuentasServices.Crear(tipoCuenta);
            return View();
        }
    }
}
