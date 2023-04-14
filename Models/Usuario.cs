using System.ComponentModel.DataAnnotations;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="El campo debe ser un correo electrónico valido")]
        public string Email { get; set; }
        public string EmailNormalizado { get; set; }
        public string PasswordHash { get; set; }
    }
}
