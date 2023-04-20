using AutoMapper;
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
        private readonly IMapper mapper;

        public TransaccionesController(ITransaccionesService transaccionesService, IUsuariosService usuariosService, ICuentasService cuentasService, ICategoriasService categoriasService, IMapper mapper)
        {
            this._transaccionesService = transaccionesService;
            this._usuariosService = usuariosService;
            this._cuentasService = cuentasService;
            this._categoriasService = categoriasService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _usuariosService.GetUsuario();
            var transacciones = await _transaccionesService.GetByUserId(userId);
            IEnumerable<TransaccionDTO> modelo = await GetMapper(transacciones);
            return View(modelo);
        }
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = _usuariosService.GetUsuario();
            var modelo = new TransaccionDTO();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(TransaccionDTO transaccion)
        {
            var usuarioId = _usuariosService.GetUsuario();
            if (!ModelState.IsValid)
            {
                transaccion.Cuentas = await ObtenerCuentas(usuarioId);
                return View(transaccion);
            }
            var cuenta = await _cuentasService.GetById(transaccion.CuentaId, usuarioId);
            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var categoria = await _categoriasService.GetById(transaccion.CategoriaId, usuarioId);
            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            transaccion.UsuarioId = usuarioId;
            if (transaccion.TipoOperacionId == (int)TipoOperacion.Gasto)
            {
                transaccion.Monto *= -1;
            }
            await _transaccionesService.Crear(transaccion);
            return RedirectToAction("Index");
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
        private async Task<IEnumerable<TransaccionDTO>> GetMapper(IEnumerable<Transaccion> transaccion)
        {
            List<TransaccionDTO> modelo = new List<TransaccionDTO>();
            foreach (var tran in transaccion)
            {
                var tranDTO = mapper.Map<TransaccionDTO>(tran);
                tranDTO.cuenta = await _cuentasService.GetById(tran.CuentaId, tran.UsuarioId);
                tranDTO.categoria = await _categoriasService.GetById(tran.CategoriaId, tran.UsuarioId);
                modelo.Add(tranDTO);
            }
            return modelo;
        }

    }
}
