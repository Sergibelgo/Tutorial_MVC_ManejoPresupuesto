using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly ITransaccionesService _transaccionesService;
        private readonly IUsuariosService _usuariosService;
        private readonly ICuentasService _cuentasService;
        private readonly ICategoriasService _categoriasService;

        public TransaccionesController(ITransaccionesService transaccionesService, IUsuariosService usuariosService, ICuentasService cuentasService, ICategoriasService categoriasService)
        {
            this._transaccionesService = transaccionesService;
            this._usuariosService = usuariosService;
            this._cuentasService = cuentasService;
            this._categoriasService = categoriasService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Crear()
        {
            var usuarioId = _usuariosService.GetUsuario();
            var modelo = new TransaccionDTO();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            return View(modelo);

        }
        [HttpPost]
        public async Task<IActionResult> ObtenerCategoriasForm([FromBody] TipoOperacion operacion)
        {
            var userId = _usuariosService.GetUsuario();
            var categorias = await ObtenerCategorais(userId, operacion);
            return Ok(categorias);
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerCategorais(int usuarioId, TipoOperacion tipoOperacion)
        {
            var categorias = await _categoriasService.GetByOperation(usuarioId, tipoOperacion);
            return categorias.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
        {
            var cuentas = await _cuentasService.GetByUserId(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.Descripcion, x.Id.ToString()));
        }

    }
}
