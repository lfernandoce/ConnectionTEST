using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace ConnectionTEST
{
    class DbMySQL
    {
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
        public List<MySqlParameter> Parametros = new List<MySqlParameter>();
        public string Database { get; set; }
        public object Resultado { get; set; }

        private readonly string StringCon;
        /// <summary>
        /// Metodo princ    ipal para establecer coneccion con base de datos;
        /// </summary>
        /// <param name="StringCon">Cadena de coneccion</param>
        public DbMySQL(string StringCon)
        {
            this.StringCon = StringCon;
        }
        /// <summary>
        /// Metodo para instanciar coneccion a base de datos SQLServer
        /// </summary>
        /// <param name="strCon"></param>
        /// <returns></returns>
        public MySqlConnection GetConnector(string strCon)
        {
            return new MySqlConnection(strCon);
        }
        /// <summary>
        /// Metodo para inicializar lista de parametros SQL
        /// </summary>
        /// <param name="ParameterName">Nombre de parametro (string)</param>
        /// <param name="Value">Valor de parametro (Object)</param>
        public void AgregarParametroWithValue(string ParameterName, Object Value)
        {
            if (Parametros == null)
            {
                Parametros = new List<MySqlParameter>();
            }
            Parametros.Add(new MySqlParameter(ParameterName, Value));
        }
        /// <summary>
        /// Metodo para ejecutar consulta a base de datos con las configuraciones inicializadas a la clase.
        /// </summary>
        /// <returns>Retorna valor booleano (true) si todo es correcto</returns>
        public object EjecutarSQL()
        {
            try
            {
                string strCon = this.StringCon;
                MySqlConnection con = GetConnector(strCon);
                con.Open();
                MySqlCommand cmd = new MySqlCommand(Query, con);
                cmd.Parameters.Clear();
                foreach (MySqlParameter p in Parametros)
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
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
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
