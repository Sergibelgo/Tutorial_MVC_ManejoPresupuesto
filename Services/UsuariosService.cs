using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface IUsuariosService
    {
        int GetUsuario();
    }
    public class UsuariosService:IUsuariosService
    {
        private readonly HttpContext _httpContext;

        public UsuariosService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContext = httpContextAccessor.HttpContext;
        }
        public int GetUsuario()
        {
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim=_httpContext.User.Claims.Where(x=>x.Type==ClaimTypes.NameIdentifier).FirstOrDefault();
                var id= int.Parse(idClaim.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("El usuario no está autenticado");
            }
        }
    }
}
