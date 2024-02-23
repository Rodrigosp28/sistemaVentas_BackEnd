using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHelper
{
    public interface IDataBase
    {
        Task<DataTable> ejecutaStoredDataTable(string sql, params SqlParameter[] parameters);
        Task<DataTable> ejecutaQueryDataTable(string sql);
        Task<DataSet> ejecutaStoredDataSet(string sql, params SqlParameter[] parameters);
        void agregaParametros<T>(ref List<SqlParameter> parametros, string key, T value);
    }
    public class BD:IDataBase
    {
        private readonly IConfiguration configuracion;

        public BD(IConfiguration con)
        {
            this.configuracion = con;
        }

        public string getConnection()
        {
            string connection = configuracion["connectionStrings:defaultConnectionString"];
            return connection;
        }

        public async Task<DataTable> ejecutaStoredDataTable(string sql, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(getConnection()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }

        }

        public async Task<DataTable> ejecutaQueryDataTable(string sql)
        {
            using (var connection = new SqlConnection(getConnection()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }

        }

        public async Task<DataTable> ejecutaQueryInsertDataTable(string sql)
        {
            using (var connection = new SqlConnection(getConnection()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }

        }

        public async Task<DataSet> ejecutaStoredDataSet(string sql, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(getConnection()))
            {

                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
        }

        public void agregaParametros<T>(ref List<SqlParameter> parametros, string key, T value)
        {
            var param = new SqlParameter();
            param.ParameterName = key;
            param.Value = value;
            parametros.Add(param);
        }
    }
}
