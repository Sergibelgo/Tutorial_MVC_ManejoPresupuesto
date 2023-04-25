using Dapper;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface IReporteService
    {
        Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerReporteSemanal(int usuarioId, int mes, int año, dynamic ViewBag);
        Task<IEnumerable<TransaccionDTO>> ObtenerReporteTransaccionesDetalladas(int usuarioId, int mes, int año, dynamic ViewBag);
        Task<IEnumerable<TransaccionDTO>> ObtenerReporteTransaccionesDetalladasPorCuenta(int usuarioId, int cuentaId, int mes, int año, dynamic ViewBag);
    }
    public class ReporteService : IReporteService
    {
        private readonly ITransaccionesService _transaccionesService;

        public ReporteService(ITransaccionesService transaccionesService)
        {
            this._transaccionesService = transaccionesService;
        }
        public async Task<IEnumerable<TransaccionDTO>> ObtenerReporteTransaccionesDetalladas(int usuarioId, int mes, int año, dynamic ViewBag)
        {
            (DateTime fechaInicio, DateTime fechaFin) = GenerarFechaInicioFechaFin(mes, año);
            var parametro = ObtenerParametrosPorUsuario(usuarioId, fechaInicio, fechaFin);
            var transacciones = (await _transaccionesService.GetByUserId(parametro)).OrderBy(x => x.FechaTransaccion);
            AsignarValoresViewBag(ViewBag, fechaInicio);
            return transacciones;
        }
        public async Task<IEnumerable<TransaccionDTO>> ObtenerReporteTransaccionesDetalladasPorCuenta(int usuarioId, int cuentaId, int mes, int año, dynamic ViewBag)
        {
            (DateTime fechaInicio, DateTime fechaFin) = GenerarFechaInicioFechaFin(mes, año);
            var obtenerTransacciones = new ObtenerTransaccionesPorCuenta()
            {
                CuentaId = cuentaId,
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };
            var transacciones = (await _transaccionesService.ObtenerPorCuentaId(obtenerTransacciones)).OrderBy(x=>x.FechaTransaccion).ToList();
            AsignarValoresViewBag(ViewBag, fechaInicio);
            return transacciones;
        }
        public async Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerReporteSemanal(int usuarioId,int mes, int año,dynamic ViewBag)
        {
            (DateTime fechaInicio, DateTime fechaFin) = GenerarFechaInicioFechaFin(mes, año);
            var parametro = ObtenerParametrosPorUsuario(usuarioId,fechaInicio,fechaFin);
            AsignarValoresViewBag(ViewBag,fechaInicio);
            var modelo = await _transaccionesService.ObtenerPorSemanas(parametro);
            return modelo;
        }
        private ParametroObtenerTransaccionesPorUsuario ObtenerParametrosPorUsuario(int usuarioId,DateTime fechaInicio,DateTime fechaFin)
        {
            return new ParametroObtenerTransaccionesPorUsuario()
            {
                UsuarioId = usuarioId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };
        }
        private (DateTime fechaInicio, DateTime fechaFin) GenerarFechaInicioFechaFin(int mes, int ano)
        {
            DateTime fechaInicio;
            DateTime fechaFin;
            if (mes <= 0 || mes > 12 || ano <= 1900)
            {
                var hoy = DateTime.Today;
                fechaInicio = new DateTime(hoy.Year, hoy.Month, 1);
            }
            else
            {
                fechaInicio = new DateTime(ano, mes, 1);
            }
            fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            return (fechaInicio, fechaFin);
        }
        private static void AsignarValoresViewBag(dynamic ViewBag, DateTime fechaInicio)
        {
            ViewBag.mesAnterior = fechaInicio.AddMonths(-1).Month;
            ViewBag.añoAnterior = fechaInicio.AddMonths(-1).Year;
            ViewBag.mesPosterior = fechaInicio.AddMonths(1).Month;
            ViewBag.añoPosterior = fechaInicio.AddMonths(1).Year;
            ViewBag.fechaActual = fechaInicio;
        }
    }
}
