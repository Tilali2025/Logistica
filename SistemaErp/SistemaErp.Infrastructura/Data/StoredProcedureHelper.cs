using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SistemaErp.Infraestructura.Data
{
    public class StoredProcedureHelper
    {
        private readonly string _connectionString;

        public StoredProcedureHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void EjecutarSP(string nombreSP, Dictionary<string, object> parametros = null)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(nombreSP, con)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parametros != null)
                foreach (var par in parametros)
                    cmd.Parameters.AddWithValue(par.Key, par.Value ?? DBNull.Value);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public DataTable EjecutarSPTabla(string nombreSP, Dictionary<string, object> parametros = null)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(nombreSP, con)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parametros != null)
                foreach (var par in parametros)
                    cmd.Parameters.AddWithValue(par.Key, par.Value ?? DBNull.Value);

            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public object EjecutarSPScalar(string nombreSP, Dictionary<string, object> parametros = null)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(nombreSP, con)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parametros != null)
                foreach (var par in parametros)
                    cmd.Parameters.AddWithValue(par.Key, par.Value ?? DBNull.Value);

            con.Open();
            return cmd.ExecuteScalar();
        }
    }
}
