using Microsoft.AspNetCore.Mvc;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriasService _categoriasService;
        private readonly IUsuariosService _usuariosService;

        public CategoriasController(ICategoriasService categoriasService, IUsuariosService usuariosService)
        {
            this._categoriasService = categoriasService;
            this._usuariosService = usuariosService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }
            var usuarioId = _usuariosService.GetUsuario();
            categoria.UsuarioId = usuarioId;
            await _categoriasService.Create(categoria);
            return RedirectToAction("Inedex");
        }
    }
}
