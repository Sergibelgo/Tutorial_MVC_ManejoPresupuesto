using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;
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
        private readonly IReporteService _reporteService;

        public TransaccionesController(ITransaccionesService transaccionesService, IUsuariosService usuariosService, ICuentasService cuentasService, ICategoriasService categoriasService, IMapper mapper, IReporteService reporteService)
        {
            this._transaccionesService = transaccionesService;
            this._usuariosService = usuariosService;
            this._cuentasService = cuentasService;
            this._categoriasService = categoriasService;
            this.mapper = mapper;
            this._reporteService = reporteService;
        }
        public async Task<IActionResult> Index(int mes, int año)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var transacciones = await _reporteService.ObtenerReporteTransaccionesDetalladas(usuarioId, mes, año, ViewBag);
            ViewBag.urlRetorno = HttpContext.Request.Path + HttpContext.Request.QueryString;
            return View(transacciones);
        }
        public async Task<IActionResult> Semanal(int mes,int año)
        {
            var usuarioId = _usuariosService.GetUsuario();
            IEnumerable<ResultadoObtenerPorSemana> transaccionesPorSemana = await _reporteService.ObtenerReporteSemanal(usuarioId, mes, año, ViewBag);
            return View();
        }
        public IActionResult Mensual()
        {
            return View();
        }
        public IActionResult ExcelReporte()
        {
            return View();
        }
        public IActionResult Calendario()
        {
            return View();
        }
        public async Task<IActionResult> Index2()
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
            var categorias = await ObtenerCategorias(userId, operacion);
            return Ok(categorias);
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var transaccion = await _transaccionesService.GetById(id, usuarioId);
            if (transaccion is null)
            {
                return View("NoEncontrado", "Home");
            }
            var modelo = mapper.Map<TransaccionDTO>(transaccion);
            if (modelo.TipoOperacionId == (int)TipoOperacion.Gasto)
            {
                modelo.MontoAnterior = modelo.Monto * -1;
            }
            modelo.CuentaAnteriorId = transaccion.CuentaId;
            modelo.Categorias = await ObtenerCategorias(usuarioId, (TipoOperacion)transaccion.TipoOperacionId);
            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(TransaccionDTO modelo)
        {
            var usuarioId = _usuariosService.GetUsuario();
            if (!ModelState.IsValid)
            {
                modelo.Categorias = await ObtenerCategorias(usuarioId, (TipoOperacion)modelo.TipoOperacionId);
                modelo.Cuentas = await ObtenerCuentas(usuarioId);
                return View(modelo);
            }
            var cuenta = await _cuentasService.GetById(modelo.CuentaId, usuarioId);
            if (cuenta is null)
            {
                return View("NoEncontrado", "Home");
            }
            var categoria = await _categoriasService.GetById(modelo.CategoriaId, usuarioId);
            if (categoria is null)
            {
                return View("NoEncontrado", "Home");
            }
            var transaccion = mapper.Map<Transaccion>(modelo);
            modelo.MontoAnterior = modelo.Monto;
            if (modelo.TipoOperacionId == (int)TipoOperacion.Gasto)
            {
                transaccion.Monto *= -1;
            }
            await _transaccionesService.Actualizar(transaccion, modelo.MontoAnterior, modelo.CuentaAnteriorId);
            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerCategorias(int usuarioId, TipoOperacion tipoOperacion)
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
