using System.ComponentModel.DataAnnotations;
using Tutorial2ManejoPresupuesto.Attributes;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        //Para los {x} el 0 es siempre el nombre del campo y los siguientes numeros son el orden de la condicion (Por ejemplo {1} es el maximo de stringLength
        [Required(ErrorMessage ="El {0} es requerido")]
        [StringLength(maximumLength: 255,MinimumLength =3,ErrorMessage ="La longitud del campo {0} debe estar entre {2} y {1}")]
        //Permite cambiar el valor del nombre al mostrar se el atributo, es decir en este caso en vez de salir "nombre" saldra "nombre del tipo de cuenta"
        [Display(Name ="Nombre del tipo cuenta")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public int Orden { get; set; }
    }
}
