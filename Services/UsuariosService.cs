namespace Tutorial2ManejoPresupuesto.Services
{
    public interface IUsuariosService
    {
        int GetUsuario();
    }
    public class UsuariosService:IUsuariosService
    {
        public int GetUsuario()
        {
            return 1;
        }
    }
}
