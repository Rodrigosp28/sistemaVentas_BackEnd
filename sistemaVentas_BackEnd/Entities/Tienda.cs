namespace sistemaVentas_BackEnd.Entities
{
    public class Tienda
    {
        public int id_tienda { get; set; }
        public string nombre { get; set; }
        public string sucursal { get; set; }
        public string direccion { get; set; }
        public DateTime fec_reg { get; set; }
        public int activo { get; set; }
    }
}
