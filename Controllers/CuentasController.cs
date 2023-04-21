using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITransaccionesService _transaccionesService;

        public ITiposCuentasService _tiposCuentasService { get; set; }
        public IUsuariosService _usuariosService { get; set; }
        public ICuentasService _cuentasService { get; set; }
        public CuentasController(ITiposCuentasService tiposCuentasService, IUsuariosService usuariosService, ICuentasService cuentasService, IMapper mapper, ITransaccionesService transaccionesService)
        {
            _tiposCuentasService = tiposCuentasService;
            _usuariosService = usuariosService;
            _cuentasService = cuentasService;
            //Mapper
            this.mapper = mapper;
            _transaccionesService = transaccionesService;
        }
        public async Task<IActionResult> Index()
        {
            int userId = _usuariosService.GetUsuario();
            var cuentasConTipoCuenta = await _cuentasService.GetByUserId(userId);
            var modelo = cuentasConTipoCuenta.GroupBy(x => x.TipoCuenta).Select(grupo => new IndiceCuentasDTO
            {
                TipoCuenta = grupo.Key,
                Cuentas = grupo.AsEnumerable()
            }).ToList();
            return View(modelo);
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
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var userId = _usuariosService.GetUsuario();
            var cuenta = await _cuentasService.GetById(id, userId);
            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            //Usar automapper
            var modelo = mapper.Map<CuentaDTO>(cuenta);
            modelo.TiposCuentas = await ObtenerTiposCuentas();
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(CuentaDTO cuentaEditar)
        {
            var userId = _usuariosService.GetUsuario();
            var cuenta = await _cuentasService.GetById(cuentaEditar.Id, userId);
            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var tipoCuenta = await _tiposCuentasService.GetTipoCuentaById(cuentaEditar.TipoCuentaId);
            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _cuentasService.Update(cuentaEditar);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var cuenta = await _cuentasService.GetById(id, usuarioId);
            if (cuenta is null || cuenta.UsuarioId != usuarioId)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(cuenta);
        }
        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var cuenta = await _cuentasService.GetById(id, usuarioId);
            if (cuenta is null || cuenta.UsuarioId != usuarioId)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _cuentasService.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Detalle(int id, int mes, int año)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var cuenta = await _cuentasService.GetById(id, usuarioId);
            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            DateTime fechaInicio;
            DateTime fechaFin;
            if (mes <= 0 || mes > 12 || año <= 1900)
            {
                var hoy = DateTime.Today;
                fechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            }
            else
            {
                fechaInicio = new DateTime(año, mes, 1);
            }
            fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            var obtenerTransacciones = new ObtenerTransaccionesPorCuenta()
            {
                CuentaId = id,
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };
            var transacciones = _transaccionesService.ObtenerPorCuentaId(obtenerTransacciones);
            
            return View(transacciones);
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas()
        {
            var tiposCuentas = await _tiposCuentasService.GetAll();
            return tiposCuentas.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(x.Nombre, x.Id.ToString()));
        }

    }
}
