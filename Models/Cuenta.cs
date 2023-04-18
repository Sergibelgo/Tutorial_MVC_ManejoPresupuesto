﻿using System.ComponentModel.DataAnnotations;
using Tutorial2ManejoPresupuesto.Attributes;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        [Display(Name = "Tipo Cuenta")]
        public int TipoCuentaId { get; set; }
        public decimal Balance { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Descripcion { get; set; }
        public int UsuarioId { get; set; }
    }
}
