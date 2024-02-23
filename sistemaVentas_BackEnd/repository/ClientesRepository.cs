using DataBaseHelper;
using sistemaVentas_BackEnd.Entities;
using sistemaVentas_BackEnd.Entities.Responses;
using System.Data;
using System.Data.SqlClient;

namespace sistemaVentas_BackEnd.repository
{
    public interface IClientes
    {
        Task<responseBase<List<Clientes>>> obtenerClientes();
        Task<responseBase<Clientes>> obtenerClientebyId(int id);
        Task<responseBase<Clientes>> insertarCliente(Clientes cliente);
        Task<responseBase<Clientes>> sesionCliente(Clientes cliente);


    }
    public class ClientesRepository:IClientes
    {
        public readonly IDataBase _BD;
        public ClientesRepository(IDataBase bd)
        {
            _BD=bd;
        }

        public async Task<responseBase<Clientes>> insertarCliente(Clientes cliente)
        {
            var response = new responseBase<Clientes>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.CLIENTES_INSERTAR";

                List<SqlParameter> parametros = new List<SqlParameter>();
                _BD.agregaParametros(ref parametros, "@nombre", cliente.nombre);
                _BD.agregaParametros(ref parametros, "@apellidos", cliente.apellidos);
                _BD.agregaParametros(ref parametros, "@direccion", cliente.direccion);
                _BD.agregaParametros(ref parametros, "@usuario", cliente.usuario);
                _BD.agregaParametros(ref parametros, "@contrasena", cliente.contrasena);

                dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());

                if (dtb != null &&dtb.Rows.Count>0)
                {
                    int resultado = Convert.ToInt32(dtb.Rows[0]["RESULTADO"]);
                    string mensaje = dtb.Rows[0]["MENSAJE"].ToString();
                    if(resultado==1)
                    {
                        response.success = true;
                        response.message = "Datos Insertados Correctamente";
                        response.data = cliente;
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
                response.message=ex.Message;
            }
            return response;
        }

        public async Task<responseBase<Clientes>> obtenerClientebyId(int id)
        {
            var response = new responseBase<Clientes>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.CLIENTES_OBTENER_ID";

                List<SqlParameter> parametros = new List<SqlParameter>();
                _BD.agregaParametros(ref parametros, "@id_cliente", id);

                dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());
                if (dtb != null && dtb.Rows.Count > 0)
                {
                    response.data = new Clientes();
                    var cliente = new Clientes();
                    cliente.id_cliente = Convert.ToInt32(dtb.Rows[0]["id_cliente"]);
                    cliente.nombre = dtb.Rows[0]["nombre"].ToString();
                    cliente.apellidos = dtb.Rows[0]["apellidos"].ToString();
                    cliente.direccion = dtb.Rows[0]["direccion"].ToString();
                    cliente.usuario = dtb.Rows[0]["usuario"].ToString();
                    cliente.fec_reg = Convert.ToDateTime(dtb.Rows[0]["fec_reg"].ToString());
                    cliente.activo = Convert.ToInt32(dtb.Rows[0]["activo"]);

                    response.data = cliente;
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

        public async Task<responseBase<List<Clientes>>> obtenerClientes()
        {
            var response = new responseBase<List<Clientes>>();
            try
            {
                var dtb = new DataTable();

                var stored = "CATALOGOS.CLIENTES_OBTENER";

                dtb = await _BD.ejecutaStoredDataTable(stored, null);

                if (dtb != null)
                {
                    response.data = new List<Clientes>();
                    foreach (DataRow d in dtb.Rows)
                    {
                        var cliente = new Clientes();
                        cliente.id_cliente = Convert.ToInt32(d["id_cliente"]);
                        cliente.nombre = d["nombre"].ToString();
                        cliente.apellidos = d["apellidos"].ToString();
                        cliente.direccion = d["direccion"].ToString();
                        cliente.usuario = d["usuario"].ToString();
                        cliente.fec_reg = Convert.ToDateTime(d["fec_reg"].ToString());
                        cliente.activo = Convert.ToInt32(d["activo"]);

                        response.data.Add(cliente);
                    }
                    response.success=true;
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

        public async Task<responseBase<Clientes>> sesionCliente(Clientes cliente)
        {
            var response = new responseBase<Clientes>();
            try
            {
                var dtb = new DataTable();

                var stored = "OPERACION.INICIO_SESION";

                List<SqlParameter> parametros = new List<SqlParameter>();
                _BD.agregaParametros(ref parametros, "@usuario", cliente.usuario);
                _BD.agregaParametros(ref parametros, "@contrasena", cliente.contrasena);


                dtb = await _BD.ejecutaStoredDataTable(stored, parametros.ToArray());

                if (dtb != null && dtb.Rows.Count > 0)
                {
                    int resultado = Convert.ToInt32(dtb.Rows[0]["RESULTADO"]);
                    string mensaje = dtb.Rows[0]["MENSAJE"].ToString();
                    if (resultado == 1)
                    {
                        cliente.id_cliente = Convert.ToInt32(dtb.Rows[0]["ID_USUARIO"]);
                        cliente.nombre = dtb.Rows[0]["NOMBRE_USUARIO"].ToString();
                        response.success = true;
                        response.message = "Sesion Iniciada Correctamente";
                        response.data = cliente;
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
    }
}
