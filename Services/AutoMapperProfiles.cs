using AutoMapper;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    //La clase hereda de Profile
    public class AutoMapperProfiles:Profile
    {
        //En el constructor añadir CreateMap<Origen,Destino>() para configurar mapeo de clase a clase
        //En el controlador que se vaya a usar hay que importarlo(mirar cuentasController)!!!
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta,CuentaDTO>();
        }
    }
}
