using System.ComponentModel.DataAnnotations;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.EmailAddress,ErrorMessage ="El campo {0} ha de ser de tipo {1}")]
        public string Email { get; set; }
        [Required (ErrorMessage ="El campo {0} es requerido")]
        [DataType(DataType.Password, ErrorMessage = "El campo {0} ha de ser de tipo {1}")]
        public string Password { get; set; }
        public bool Recuerdame { get; set; }
    }
}
