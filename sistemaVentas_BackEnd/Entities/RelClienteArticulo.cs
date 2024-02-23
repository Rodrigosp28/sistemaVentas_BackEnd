namespace sistemaVentas_BackEnd.Entities
{
    public class RelClienteArticulo
    {
        public int id_cliente { get; set; }
        public int id_articulo { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public double total { get; set; }
        public DateTime fec_reg { get; set; }
    }
}
