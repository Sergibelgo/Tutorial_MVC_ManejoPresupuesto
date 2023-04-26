using Microsoft.AspNetCore.Identity;

namespace Tutorial2ManejoPresupuesto.Services
{
    public class MensajesDeErrorIdentity : IdentityErrorDescriber
    {
        //Estos son solo algunos ejemplos, para ver más al poner public override visual te muestra los metodos que hay
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = nameof(InvalidEmail), Description = "El email no es valido" };
        }
        public override IdentityError DefaultError()
        {
            return new IdentityError { Code = nameof(DefaultError), Description = "Ha ocurrido un error" };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = nameof(DuplicateEmail), Description = "El email ya existe en la plataforma" };
        }
    }
}
