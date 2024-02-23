using DataBaseHelper;
using sistemaVentas_BackEnd.Entities;
using sistemaVentas_BackEnd.Entities.Responses;
using System.Data.SqlClient;
using System.Data;

namespace sistemaVentas_BackEnd.repository
{
    public interface ITienda
    {
        Task<responseBase<List<Tienda>>> obtenerTiendas();
        Task<responseBase<Tienda>> obtenerTiendabyId(int id);
        Task<responseBase<Tienda>> insertarTienda(Tienda tienda);
    }
    public class TiendaRepository:ITienda
    {
        public readonly IDataBase _BD;
        public TiendaRepository(IDataBase bd)
        {
            _BD = bd;
        }

        public async Task<responseBase<Tienda>> insertarTienda(Tienda tienda)
        {
            var response = new responseBase<Tienda>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.TIENDA_INSERTA";

                List<SqlParameter> parametros = new List<SqlParameter>();
                _BD.agregaParametros(ref parametros, "@nombre", tienda.nombre);
                _BD.agregaParametros(ref parametros, "@sucursal", tienda.nombre);
                _BD.agregaParametros(ref parametros, "@direccion", tienda.direccion);

                dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());

                if (dtb != null && dtb.Rows.Count > 0)
                {
                    int resultado = Convert.ToInt32(dtb.Rows[0]["RESULTADO"]);
                    string mensaje = dtb.Rows[0]["MENSAJE"].ToString();
                    if (resultado == 1)
                    {
                        response.success = true;
                        response.message = "Datos Insertados Correctamente";
                        response.data = tienda;
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
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<responseBase<Tienda>> obtenerTiendabyId(int id)
        {
            var response = new responseBase<Tienda>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.TIENDA_OBTENER_ID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                _BD.agregaParametros(ref parametros, "@id_tienda", id);

                dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());
                if (dtb != null && dtb.Rows.Count > 0)
                {
                    response.data = new Tienda();
                    var tienda = new Tienda();
                    tienda.id_tienda = Convert.ToInt32(dtb.Rows[0]["id_tienda"]);
                    tienda.nombre = dtb.Rows[0]["nombre"].ToString();
                    tienda.sucursal = dtb.Rows[0]["sucursal"].ToString();
                    tienda.direccion = dtb.Rows[0]["direccion"].ToString();
                    tienda.fec_reg = Convert.ToDateTime(dtb.Rows[0]["fec_reg"].ToString());
                    tienda.activo = Convert.ToInt32(dtb.Rows[0]["activo"]);

                    response.data = tienda;
                    response.success = true;
                    response.message = "Datos Obtenidos Correctamente";
                    response.id = 1;
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

        public async Task<responseBase<List<Tienda>>> obtenerTiendas()
        {
            var response = new responseBase<List<Tienda>>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.TIENDA_OBTENER";

                dtb = await _BD.ejecutaStoredDataTable(stored, null);

                if (dtb != null)
                {
                    response.data = new List<Tienda>();
                    foreach (DataRow d in dtb.Rows)
                    {
                        var tienda = new Tienda();
                        tienda.id_tienda = Convert.ToInt32(d["id_tienda"]);
                        tienda.nombre = d["nombre"].ToString();
                        tienda.sucursal = d["sucursal"].ToString();
                        tienda.direccion = d["direccion"].ToString();
                        tienda.fec_reg = Convert.ToDateTime(d["fec_reg"].ToString());
                        tienda.activo = Convert.ToInt32(d["activo"]);

                        response.data.Add(tienda);
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
