﻿using System.ComponentModel.DataAnnotations;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:50,ErrorMessage ="No puede ser mayor a {1} caracteres")]
        public string Nombre { get; set; }
        public TipoOperacion TipoOperacion { get; set; }
        public int UserId { get; set; }
    }
}
