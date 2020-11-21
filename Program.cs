using System;
using System.Data;

namespace ConnectionTEST
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ejemplo de una coneccion a SQLSERVER
            String StrCon = "data source=VSR-DEVCOOKIE\\DEVCOOKIE;initial catalog=MvcMovieContext;persist security info=True;user id=sa;password=LFCE_2110;MultipleActiveResultSets=True;App=ConnectionTEST";
            DbSQL db = new DbSQL(StrCon);
            db.Query = "SELECT * FROM TAB_USUARIOS_REGISTRADOS";
            db.Database = "DB_TESTS";
            db.Exec = DbSQL.Execute.Fill;
            db.CommandType = System.Data.CommandType.Text;
            db.EjecutarSQL();
            System.Console.ReadKey();




            //Ejemplo de coneccion a oracle, utilizamos oracle 10g
            string strConOra = "Data Source=localhost:1521; User Id=system;Password=2110;";
            DbORACLE Oradb = new DbORACLE(strConOra);
            Oradb.Query = "SELECT * FROM TAB_USUARIOS";
            //Oradb.Database = "xe";
            Oradb.Exec =DbORACLE.Execute.Fill;
            Oradb.CommandType = System.Data.CommandType.Text;
            DataTable dt = new DataTable();
            dt=(DataTable)Oradb.EjecutarSQL();
            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine(dr[0].ToString());
            }
            System.Console.ReadKey();


            // Ejemplo de coneccion a MySQL
            string strConMysql = "Server=localhost;Database=Test;Uid=root;Pwd='';";
            DbMySQL MySqldb = new DbMySQL(strConMysql);
            MySqldb.Query = "SELECT * FROM USUARIOS_TEST";            
            MySqldb.Exec = DbMySQL.Execute.Fill;
            MySqldb.CommandType = System.Data.CommandType.Text;
            DataTable dt2 = new DataTable();
            dt = (DataTable)MySqldb.EjecutarSQL();
            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine(dr[0].ToString());
            }
            System.Console.ReadKey();


        
        }
    }
}
