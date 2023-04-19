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
        public async Task<IActionResult> Index()
        {
            var usuarioId = _usuariosService.GetUsuario();
            var categorias = await _categoriasService.GetByUserId(usuarioId);
            return View(categorias);
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
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var categoria = await _categoriasService.GetById(id, usuarioId);
            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(categoria);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoriaEditar)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaEditar);
            }
            var usuairoId = _usuariosService.GetUsuario();
            var categoria = await _categoriasService.GetById(categoriaEditar.Id, usuairoId);
            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            categoriaEditar.UsuarioId = usuairoId;
            await _categoriasService.Update(categoriaEditar);
            return RedirectToAction("Index");
        }
    }
}
