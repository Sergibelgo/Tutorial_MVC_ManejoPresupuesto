namespace Tutorial2ManejoPresupuesto.Models
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina { get; set; } = 10;
        private readonly int cantidadMaximaRecordsPorPagina = 50;
        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            }
        }
        public int RecordsASaltar => recordsPorPagina * (Pagina - 1);
    }
}
