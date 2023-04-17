using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly string _configuration;
        public TiposCuentasController(IConfiguration IConfiguration)
        {
            _configuration = IConfiguration.GetConnectionString("DefaultConnection");
        }
        [HttpGet]
        public IActionResult Crear()
        {
            using (var connection = new SqlConnection(_configuration))
            {
                var query = connection.Query("SELECT 1").FirstOrDefault();
            }
            return View();
        }
        [HttpPost]
        public IActionResult Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            return View();
        }
    }
}
