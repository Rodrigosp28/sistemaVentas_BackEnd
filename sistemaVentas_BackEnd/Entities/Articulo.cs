namespace sistemaVentas_BackEnd.Entities
{
    public class Articulo
    {
        public int id_articulo { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public string url_img { get; set; }
        public int stock { get; set; }
        public DateTime fec_reg { get; set; }
        public int activo { get; set; }
        public int id_tienda { get; set; } = 0;
        public string nombre_tienda { get; set; } = "";
    }
}
