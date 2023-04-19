using System.ComponentModel.DataAnnotations;
using Tutorial2ManejoPresupuesto.Attributes;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        public string TipoCuenta { get; set; }
        [Required]
        [Display(Name = "Tipo Cuenta")]
        public int TipoCuentaId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [StringLength(maximumLength: 1000)]
        public string Descripcion { get; set; }
        public int UsuarioId { get; set; }
    }
}
