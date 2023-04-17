using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tutorial2ManejoPresupuesto.Attributes;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class TipoCuenta:ValidationAttribute
    {
        public int Id { get; set; }
        //Para los {x} el 0 es siempre el nombre del campo y los siguientes numeros son el orden de la condicion (Por ejemplo {1} es el maximo de stringLength
        [Required(ErrorMessage ="El {0} es requerido")]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 30,MinimumLength =3,ErrorMessage ="La longitud del campo {0} debe estar entre {2} y {1}")]
        //Permite cambiar el valor del nombre al mostrar se el atributo, es decir en este caso en vez de salir "nombre" saldra "nombre del tipo de cuenta"
        [Display(Name ="Nombre del tipo cuenta")]
        //[Remote(action: "VerificarExisteTipoCuenta",controller:"TiposCuentas")]
        public string Nombre { get; set; }
        public int Orden { get; set; }
        //permite cambiar las reglas de validacion del modelo
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Nombre!=null && Nombre.Length>0)
        //    {
        //        var primeraLetra = Nombre[0].ToString();
        //        if (primeraLetra!=primeraLetra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayúscula", new[] { nameof(Nombre) });
        //        }
        //        else
        //        {
        //            yield return ValidationResult.Success;
        //        }
        //    }
        //}
    }
}
