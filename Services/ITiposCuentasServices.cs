﻿using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface ITiposCuentasServices
    {
        public Task Crear(TipoCuenta tipoCuenta);
        public Task<bool> IsPresent(string nombre);
    }
}
