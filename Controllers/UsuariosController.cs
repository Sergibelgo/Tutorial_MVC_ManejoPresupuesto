using Microsoft.AspNetCore.Mvc;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroDTO regitro)
        {
            if (!ModelState.IsValid)
            {
                return View(regitro);
            }
            return RedirectToAction("Index", "Transacciones");
        }
    }
}
