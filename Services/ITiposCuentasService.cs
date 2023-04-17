using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface ITiposCuentasService
    {
        public Task Crear(TipoCuenta tipoCuenta);
        public Task<bool> IsPresent(string nombre);
        public Task<IEnumerable<TipoCuenta>> GetAll();
        Task Update(TipoCuenta tipoCuenta);
        Task<TipoCuenta> GetTipoCuentaById(int id);
    }
}
