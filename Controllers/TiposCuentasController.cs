using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly ITiposCuentasService _tiposCuentasServices;
        private readonly IUsuariosService _usuariosService;
        public TiposCuentasController(ITiposCuentasService tiposCuentasServices, IUsuariosService usuariosService)
        {
            _tiposCuentasServices = tiposCuentasServices;
            _usuariosService = usuariosService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tiposCuentas = await _tiposCuentasServices.GetAll();
            return View(tiposCuentas);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var tipoCuenta = await _tiposCuentasServices.GetTipoCuentaById(id);
            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var tipoCuentaExiste = await _tiposCuentasServices.GetTipoCuentaById(tipoCuenta.Id);
            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _tiposCuentasServices.Update(tipoCuenta);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> DeleteCon(int Id)
        {
            var tipoCuenta = await _tiposCuentasServices.GetTipoCuentaById(Id);
            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var tipoCuenta = await _tiposCuentasServices.GetTipoCuentaById(Id);
            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _tiposCuentasServices.Delete(Id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            var exists = await _tiposCuentasServices.IsPresent(tipoCuenta.Nombre);
            if (exists)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }
            await _tiposCuentasServices.Crear(tipoCuenta);
            return RedirectToAction("Index");
        }
        [HttpGet]
        //Esto sirve para usar la propiedad [Remote(action:"nombrePeticion",controller:"controladorDondeEsta")] que verificara el formulario
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var existe = await _tiposCuentasServices.IsPresent(nombre);
            if (existe)
            {
                //Si queremos devolver cualquier cosa se devuelve un texto con la info
                return Json($"El nombre {nombre} ya existe");
            }
            //Si esta bien se devuelve true directamente
            return Json(true);
        }
        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var tiposCuentas = await _tiposCuentasServices.GetAll();
            var idsTiposCuentas = tiposCuentas.Select(x => x.Id).ToList();
            if (idsTiposCuentas.Count() != ids.Count())
            {
                return Forbid();
            }
            Console.WriteLine(ids[0]);
            var tiposCuentasOrdenados = ids.Select((valor, indice) => new TipoCuenta() { Id = valor, Orden = indice + 1 }).AsEnumerable();
            await _tiposCuentasServices.Ordenar(tiposCuentasOrdenados);
            return Ok();
        }
    }
}
