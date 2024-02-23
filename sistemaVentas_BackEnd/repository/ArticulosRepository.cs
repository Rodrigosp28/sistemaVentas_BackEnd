using DataBaseHelper;
using sistemaVentas_BackEnd.Entities;
using sistemaVentas_BackEnd.Entities.Responses;
using System.Data.SqlClient;
using System.Data;

namespace sistemaVentas_BackEnd.repository
{
    public interface IArticulo
    {
        Task<responseBase<List<Articulo>>> obtenerArticulos();
        Task<responseBase<Articulo>> obtenerArticulosbyId(int id);
        Task<responseBase<List<Articulo>>> obtenerArticulosbyIdTienda(int id);
        Task<responseBase<Articulo>> insertarArticulo(Articulo articulo);

        Task<responseMain> insertarArticuloCliente(List<RelClienteArticulo> articulos);
    }
    public class ArticulosRepository:IArticulo
    {
        public readonly IDataBase _BD;
        public ArticulosRepository(IDataBase bd)
        {
            _BD = bd;
        }

        public async Task<responseBase<Articulo>> insertarArticulo(Articulo articulo)
        {
            var response = new responseBase<Articulo>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.ARTICULOS_INSERTAR";

                List<SqlParameter> parametros = new List<SqlParameter>();
                _BD.agregaParametros(ref parametros, "@codigo", articulo.codigo);
                _BD.agregaParametros(ref parametros, "@descripcion", articulo.descripcion);
                _BD.agregaParametros(ref parametros, "@precio", articulo.precio);
                _BD.agregaParametros(ref parametros, "@url_img", articulo.url_img);
                _BD.agregaParametros(ref parametros, "@stock", articulo.stock);
                _BD.agregaParametros(ref parametros, "@id_tienda", articulo.id_tienda);

                dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());


                if (dtb != null && dtb.Rows.Count > 0)
                {
                    int resultado = Convert.ToInt32(dtb.Rows[0]["RESULTADO"]);
                    string mensaje = dtb.Rows[0]["MENSAJE"].ToString();
                    if (resultado == 1)
                    {
                        response.success = true;
                        response.message = "Datos Insertados Correctamente";
                        response.data = articulo;
                    }
                    else
                    {
                        response.success = false;
                        response.message = mensaje;
                    }
                }
                else
                {
                    response.success = false;
                    response.message = "Error al consultar BD";
                }
            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<responseMain> insertarArticuloCliente(List<RelClienteArticulo> articulos)
        {
            var response = new responseMain();
            try
            {
                var dtb = new DataTable();

                var stored = "OPERACION.INSERTAR_REL_ARTICULO_CLIENTE";

                foreach(RelClienteArticulo articulo in articulos)
                {
                    List<SqlParameter> parametros = new List<SqlParameter>();
                    _BD.agregaParametros(ref parametros, "@id_cliente", articulo.id_cliente);
                    _BD.agregaParametros(ref parametros, "@id_articulo", articulo.id_articulo);
                    _BD.agregaParametros(ref parametros, "@cantidad", 1);

                    dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());

                }
                response.success = true;
                response.message = "Datos Insertados Correctamente";

            }
            catch(Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<responseBase<List<Articulo>>> obtenerArticulos()
        {
            var response = new responseBase<List<Articulo>>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.ARTICULOS_OBTENER";

                dtb = await _BD.ejecutaStoredDataTable(stored, null);

                if (dtb != null)
                {
                    response.data = new List<Articulo>();
                    foreach (DataRow d in dtb.Rows)
                    {
                        
                        var articulo = new Articulo();
                        articulo.id_articulo = Convert.ToInt32(d["id_articulo"]);
                        articulo.codigo = d["codigo"].ToString();
                        articulo.descripcion = d["descripcion"].ToString();
                        articulo.precio = Convert.ToDouble(d["precio"]);
                        articulo.url_img = d["url_img"].ToString();
                        articulo.stock = Convert.ToInt32(d["stock"]);
                        articulo.fec_reg = Convert.ToDateTime(d["fec_reg"].ToString());
                        articulo.activo = Convert.ToInt32(d["activo"]);
                        articulo.nombre_tienda = d["nombre_tienda"].ToString();

                        response.data.Add(articulo);
                    }
                    response.success = true;
                    response.message = "Datos Obtenidos Correctamente";
                    response.id = response.data.Count();
                }
                else
                {
                    response.id = 0;
                    response.success = false;
                    response.message = "No Se encontraron datos";
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<responseBase<Articulo>> obtenerArticulosbyId(int id)
        {
            var response = new responseBase<Articulo>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.ARTICULOS_OBTENER_ID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                _BD.agregaParametros(ref parametros, "@id_articulo", id);

                dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());
                if (dtb != null && dtb.Rows.Count > 0)
                {
                    response.data = new Articulo();
                    var articulo = new Articulo();
                    articulo.id_articulo = Convert.ToInt32(dtb.Rows[0]["id_articulo"]);
                    articulo.codigo = dtb.Rows[0]["codigo"].ToString();
                    articulo.descripcion = dtb.Rows[0]["descripcion"].ToString();
                    articulo.precio = Convert.ToDouble(dtb.Rows[0]["precio"]);
                    articulo.url_img = dtb.Rows[0]["url_img"].ToString();
                    articulo.stock = Convert.ToInt32(dtb.Rows[0]["stock"]);
                    articulo.fec_reg = Convert.ToDateTime(dtb.Rows[0]["fec_reg"].ToString());
                    articulo.activo = Convert.ToInt32(dtb.Rows[0]["activo"]);

                    response.data = articulo;
                    response.success = true;
                    response.message = "Datos Obtenidos Correctamente";
                    response.id = 1;
                }
                else
                {
                    response.id = 0;
                    response.success = true;
                    response.message = "No Se encontraron datos";
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<responseBase<List<Articulo>>> obtenerArticulosbyIdTienda(int id)
        {
            var response = new responseBase<List<Articulo>>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.ARTICULOS_OBTENER_ID_TIENDA";

                List<SqlParameter> parametros = new List<SqlParameter>();
                _BD.agregaParametros(ref parametros, "@id_tienda", id);

                dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());
                if (dtb != null)
                {
                    response.data = new List<Articulo>();
                    foreach (DataRow d in dtb.Rows)
                    {

                        var articulo = new Articulo();
                        articulo.id_articulo = Convert.ToInt32(d["id_articulo"]);
                        articulo.codigo = d["codigo"].ToString();
                        articulo.descripcion = d["descripcion"].ToString();
                        articulo.precio = Convert.ToDouble(d["precio"]);
                        articulo.url_img = d["url_img"].ToString();
                        articulo.stock = Convert.ToInt32(d["stock"]);
                        articulo.fec_reg = Convert.ToDateTime(d["fec_reg"].ToString());
                        articulo.activo = Convert.ToInt32(d["activo"]);
                        articulo.nombre_tienda = d["nombre_tienda"].ToString();

                        response.data.Add(articulo);
                    }
                    response.success = true;
                    response.message = "Datos Obtenidos Correctamente";
                    response.id = response.data.Count();
                }
                else
                {
                    response.id = 0;
                    response.success = false;
                    response.message = "No Se encontraron datos";
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
