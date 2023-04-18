using Microsoft.AspNetCore.Mvc;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        public ITiposCuentasService _tiposCuentasService { get; set; }
        public IUsuariosService _usuariosService { get; set; }
        public CuentasController(ITiposCuentasService tiposCuentasService, IUsuariosService usuariosService)
        {
            _tiposCuentasService = tiposCuentasService;
            _usuariosService = usuariosService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        async public Task<IActionResult> Crear()
        {
            var usuarioId = _usuariosService.GetUsuario();
            var tiposCuentas = await _tiposCuentasService.GetAll();
            CuentaDTO modelo = new CuentaDTO();
            modelo.TiposCuentas = tiposCuentas.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(x.Nombre, x.Id.ToString()));
            modelo.UsuarioId = usuarioId;
            return View(modelo);
        }
    }
}
