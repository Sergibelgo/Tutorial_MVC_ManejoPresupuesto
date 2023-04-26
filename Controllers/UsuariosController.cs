using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public UsuariosController(UserManager<Usuario> userManager,SignInManager<Usuario> signInManager)
        {
            this._userManager = userManager;
            //Servicio para manejar login de usuarios
            this._signInManager = signInManager;
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
                //Esto genera la cookie que guardara el usuario
                await _signInManager.SignInAsync(usuario, isPersistent:true);
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
