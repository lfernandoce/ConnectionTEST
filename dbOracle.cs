﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Microsoft.Win32.SafeHandles;
namespace ConnectionTEST
{
    class DbORACLE: IDisposable
    {

        //Variables para destruir el objeto
        private bool _disposed = false;
        private readonly SafeFileHandle _safeHandle;
        /// <summary>
        /// Metodo para destruir el objeto
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            // Dispose of managed resources here.
            if (disposing)
            {
                _safeHandle?.Dispose();
            }
            // Dispose of any unmanaged resources not wrapped in safe handles.
            _disposed = true;
        }


        private string StrConOracle;
        public enum Execute
        {
            ExecuteNonQuery
              , ExecuteScalar
              , Fill
              , DataReader
        }
        public string Query { get; set; }
        public CommandType CommandType { get; set; }
        public Execute Exec { get; set; }
        public string Mensaje { get; set; }
        public string Exeption { get; set; }
        public List<OracleParameter> Parametros = new List<OracleParameter>();
        public string Database { get; set; }
        public object Resultado { get; set; }

        //private readonly string StringCon="";


        public DbORACLE(string StrConOracle)
        {
            this.StrConOracle = StrConOracle;
        }

        public OracleConnection GetConnection()
        {
            return new OracleConnection(StrConOracle);
        }

        public void AgregarParametroWithValue(string ParameterName, Object Value)
        {
            if (Parametros == null)
            {
                Parametros = new List<OracleParameter>();
            }
            Parametros.Add(new OracleParameter(ParameterName, Value));
        }
        public object EjecutarSQL()
        {
            try
            {
                //string strCon = this.StrConOracle;
                OracleConnection con = GetConnection();
                con.Open();
                OracleCommand cmd = new OracleCommand(Query, con);
                cmd.Parameters.Clear();
                foreach (OracleParameter p in Parametros)
                {
                    cmd.Parameters.Add(p);
                }
                cmd.CommandType = CommandType;
                //cmd.Connection.ChangeDatabase(Database);

                switch (Exec)
                {
                    case Execute.ExecuteNonQuery:
                        Resultado = cmd.ExecuteNonQuery();
                        break;
                    case Execute.Fill:
                        DataTable dt = new DataTable();
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        da.Fill(dt);
                        Resultado = dt;
                        break;
                    case Execute.ExecuteScalar:
                        Resultado = cmd.ExecuteScalar();
                        break;
                    case Execute.DataReader:
                        Resultado = cmd.ExecuteReader();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mensaje: " + ex.Message);
                Mensaje = ex.Message;
                Console.WriteLine("Exception: " + ex.ToString());
                Exeption = ex.ToString();
                Resultado = null;
                throw new Exception(ex.Message, ex.InnerException);
            }
            return Resultado;
        }


    }
}
