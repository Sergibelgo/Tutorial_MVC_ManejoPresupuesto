using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;

        public UsuariosController(UserManager<Usuario> userManager)
        {
            this._userManager = userManager;
        }
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
            var usuario = new Usuario()
            {
                Email = regitro.Email
            };
            var resultado = await _userManager.CreateAsync(usuario, password: regitro.Password);
            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Transacciones");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(regitro);
            }
        }
    }
}
