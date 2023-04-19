using System.ComponentModel.DataAnnotations;

namespace Tutorial2ManejoPresupuesto.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaTransaccion { get; set; } = DateTime.Now;
        public decimal Monto { get; set; }
        public int TipoOperacionId { get; set; }
        [StringLength(maximumLength:1000,ErrorMessage ="La nota no puede pasar de {1} caracteres")]
        public string Nota { get; set; }
        [Range(1,maximum:int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        public int CuentaId { get; set; }
        [Range(1,maximum:int.MaxValue,ErrorMessage ="Debe seleccionar una categoría")]
        public int CategoriaId { get; set; }
    }
}
