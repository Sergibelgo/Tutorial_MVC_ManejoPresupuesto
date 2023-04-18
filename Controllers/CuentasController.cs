using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        public ITiposCuentasService _tiposCuentasService { get; set; }
        public IUsuariosService _usuariosService { get; set; }
        public ICuentasService _cuentasService { get; set; }
        public CuentasController(ITiposCuentasService tiposCuentasService, IUsuariosService usuariosService, ICuentasService cuentasService)
        {
            _tiposCuentasService = tiposCuentasService;
            _usuariosService = usuariosService;
            _cuentasService = cuentasService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = _usuariosService.GetUsuario();
            CuentaDTO modelo = new CuentaDTO();
            modelo.TiposCuentas = await ObtenerTiposCuentas();
            modelo.UsuarioId = usuarioId;
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(CuentaDTO cuenta)
        {
            if (!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas();
                return View(cuenta);
            }
            var tipoCuenta = await _tiposCuentasService.GetTipoCuentaById(cuenta.TipoCuentaId);
            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _cuentasService.Crear(cuenta);
            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas()
        {
            var tiposCuentas = await _tiposCuentasService.GetAll();
            return tiposCuentas.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
