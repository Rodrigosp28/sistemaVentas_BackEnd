namespace sistemaVentas_BackEnd.Entities
{
    public class Clientes
    {
        public int id_cliente { get; set; } = 0;
        public string nombre { get; set; } = "";
        public string apellidos { get; set; } = "";
        public string direccion { get; set; } = "";
        public string usuario { get; set; } = "";
        public string contrasena { get; set; } = "";
        public DateTime fec_reg { get; set; }
        public int activo { get; set; }
    }
}
