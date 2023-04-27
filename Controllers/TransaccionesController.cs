using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.IO;
using System.Transactions;
using Tutorial2ManejoPresupuesto.Models;
using Tutorial2ManejoPresupuesto.Services;

namespace Tutorial2ManejoPresupuesto.Controllers
{
    //[Authorize] Para ponerlo en todo el controlador
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
        //[Authorize] para ponerlo a endpoints concretos
        public async Task<IActionResult> Index(int mes, int año)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var transacciones = await _reporteService.ObtenerReporteTransaccionesDetalladas(usuarioId, mes, año, ViewBag);
            ViewBag.urlRetorno = HttpContext.Request.Path + HttpContext.Request.QueryString;
            TransaccionesViewDTO model = new()
            {
                transacciones = transacciones
            };
            return View(model);
        }
        public async Task<IActionResult> Semanal(int mes, int año)
        {
            var usuarioId = _usuariosService.GetUsuario();
            IEnumerable<ResultadoObtenerPorSemana> transaccionesPorSemana = await _reporteService.ObtenerReporteSemanal(usuarioId, mes, año, ViewBag);
            var agrupado = transaccionesPorSemana.GroupBy(x => x.Semana).Select(x => new ResultadoObtenerPorSemana()
            {
                Semana = x.Key,
                Ingresos = x.Where(x => x.TipoOperacionId == (int)TipoOperacion.Ingreso).Select(x => x.Monto).FirstOrDefault(),
                Gastos = x.Where(x => x.TipoOperacionId == (int)TipoOperacion.Gasto).Select(x => x.Monto).FirstOrDefault(),

            }).ToList();
            if (año == 0 || mes == 0)
            {
                var hoy = DateTime.Today;
                año = hoy.Year;
                mes = hoy.Month;
            }
            var fechaReferencia = new DateTime(año, mes, 1);
            var diasDelMes = Enumerable.Range(1, fechaReferencia.AddMonths(1).AddDays(-1).Day);
            var diasSegmentados = diasDelMes.Chunk(7).ToList();
            for (int i = 0; i < diasSegmentados.Count; i++)
            {
                var semana = i + 1;
                var fechaInicio = new DateTime(año, mes, diasSegmentados[i].First());
                var fechaFin = new DateTime(año, mes, diasSegmentados[i].Last());
                var grupoSemana = agrupado.FirstOrDefault(x => x.Semana == semana);
                if (grupoSemana is null)
                {
                    agrupado.Add(new ResultadoObtenerPorSemana()
                    {
                        Semana = semana,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin
                    });
                }
                else
                {
                    grupoSemana.FechaInicio = fechaInicio;
                    grupoSemana.FechaFin = fechaFin;
                }
            }
            agrupado = agrupado.OrderBy(x => x.Semana).ToList();
            var modelo = new ReporteSemanalDTO
            {
                TransaccionesPorSemana = agrupado,
                FechaReferencia = fechaReferencia
            };

            return View(modelo);
        }
        public async Task<IActionResult> Mensual(int año)
        {
            var usuarioId = _usuariosService.GetUsuario();
            if (año == 0)
            {
                año = DateTime.Today.Year;
            }
            var transaccionesPorMes = await _transaccionesService.ObtenerPorMes(usuarioId, año);
            var transaccionesAgrupadas = transaccionesPorMes.GroupBy(x => x.Mes).
                Select(x => new ResultadoObtenerPorMes()
                {
                    Mes = x.Key,
                    Ingreso = x.Where(x => x.TipoOperacionId == TipoOperacion.Ingreso).Select(x => x.Monto).FirstOrDefault(),
                    Gasto = x.Where(x => x.TipoOperacionId == TipoOperacion.Gasto).Select(x => x.Monto).FirstOrDefault()
                }).ToList();
            for (int mes = 1; mes <= 12; mes++)
            {
                var transaccion = transaccionesAgrupadas.FirstOrDefault(x => x.Mes == mes);
                var fechaReferencia = new DateTime(año, mes, 1);
                if (transaccion is null)
                {
                    transaccionesAgrupadas.Add(new ResultadoObtenerPorMes()
                    {
                        Mes = mes,
                        FechaReferencia = fechaReferencia
                    });
                }
                else
                {
                    transaccion.FechaReferencia = fechaReferencia;
                }
            }
            transaccionesAgrupadas = transaccionesAgrupadas.OrderBy(x => x.Mes).ToList();
            var modelo = new ReporteMensualDTO()
            {
                TransaccionesPorMes = transaccionesAgrupadas,
                Año = año,
            };
            return View(modelo);
        }
        public IActionResult ExcelReporte()
        {
            return View();
        }
        [HttpGet]
        public async Task<FileResult> ExportarExcelPorMes(int mes, int año)
        {
            var fechaInicio = new DateTime(año, mes, 1);
            var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            var usuarioId = _usuariosService.GetUsuario();
            var transacciones = await _transaccionesService.GetByUserId(new ParametroObtenerTransaccionesPorUsuario()
            {
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            });
            var nombreArchivo = $"Manejo_Presupuesto-{fechaInicio.ToString("MMM-yyyy")}.xlsx";
            return GenerarExcel(nombreArchivo, transacciones);
        }
        [HttpGet]
        public async Task<FileResult> ExportarExcelPorAño(int año)
        {
            var fechaInicio = new DateTime(año, 1, 1);
            var fechaFin = fechaInicio.AddYears(1).AddDays(-1);
            var usuarioId = _usuariosService.GetUsuario();
            var transacciones = await _transaccionesService.GetByUserId(new ParametroObtenerTransaccionesPorUsuario()
            {
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            });
            var nombreArchivo = $"Manejo Presupuesto - {fechaInicio.ToString("yyyy")}.xlsx";
            return GenerarExcel(nombreArchivo, transacciones);
        }
        [HttpGet]
        public async Task<FileResult> ExportarExcelTodo()
        {
            var fechaInicio = DateTime.Today.AddYears(-200);
            var fechaFin=DateTime.Today.AddDays(1000);
            var usuarioId = _usuariosService.GetUsuario();
            var transacciones = await _transaccionesService.GetByUserId(new ParametroObtenerTransaccionesPorUsuario()
            {
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            });
            var nombreArchivo = $"Manejo Presupuesto - {DateTime.Today.ToString("dd-MM-yyyy")}.xlsx";
            return GenerarExcel(nombreArchivo, transacciones);
        }
        [HttpGet]
        public IActionResult Calendario()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> ObtenerTransaccionesCalendario(DateTime start,DateTime end)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var transacciones = await _transaccionesService.GetByUserId(new ParametroObtenerTransaccionesPorUsuario()
            {
                UsuarioId = usuarioId,
                FechaInicio = start,
                FechaFin = end
            });
            var eventosCalendario = transacciones.Select(x => new EventoCalendario()
            {
                Title = x.Monto.ToString("N"),
                Start = x.FechaTransaccion.ToString("yyy-MM-dd"),
                End = x.FechaTransaccion.ToString("yyy-MM-dd"),
                Color=(x.TipoOperacionId==(int)TipoOperacion.Gasto)? "Red":""
            });
            return Json(eventosCalendario);
        }
        [HttpGet]
        public async Task<JsonResult> ObtenerTransaccionesPorFecha(DateTime fecha)
        {
            var usuarioId = _usuariosService.GetUsuario();
            var transacciones = await _transaccionesService.GetByUserId(new ParametroObtenerTransaccionesPorUsuario()
            {
                UsuarioId = usuarioId,
                FechaInicio = fecha,
                FechaFin = fecha
            });
            return Json(transacciones);
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
            var resultado= categorias.Select(x => new SelectListItem(x.Nombre, x.Id.ToString())).ToList();
            var opcionPorDefecto =new SelectListItem("-- Seleccione una categoría --","0",true);
            resultado.Insert(0,opcionPorDefecto);
            return resultado;
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
        private FileResult GenerarExcel(string nombreArchivo, IEnumerable<TransaccionDTO> transacciones)
        {
            DataTable dataTable = new DataTable("Transacciones");
            dataTable.Columns.AddRange(new DataColumn[] {
                new DataColumn("Fecha"),
                new DataColumn("Cuenta"),
                new DataColumn("Categoria"),
                new DataColumn("Nota"),
                new DataColumn("Monto"),
                new DataColumn("Ingreso/Gasto")
            });
            foreach (var item in transacciones)
            {
                dataTable.Rows.Add(item.FechaTransaccion, item.NombreCuenta, item.NombreCategoria, item.Nota,item.Monto, item.TipoOperacionId == (int)TipoOperacion.Ingreso ? "Ingreso" : "Gasto");
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }

        }
    }
}
