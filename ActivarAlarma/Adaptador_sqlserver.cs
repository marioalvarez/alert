using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ActivarAlarma
{
    public class Adaptador_sqlserver
    {
        #region Eventos
        //Variable de tipo delegado para crear un evento personalizado
        public delegate void DelegadoServerEventHandler(string Descripcion, int numero);
        //Evento del tipo delegado personalizado
        public event DelegadoServerEventHandler ErrorObjeto;

        #endregion;

        #region Miembros
        private string MyConnectionString = "";

        protected DataSet myDataSet;
        protected SqlConnection conn; //MySqlConnection
        protected SqlCommand comando; //MySqlCommand
        protected SqlDataAdapter myAdapter;  //MySqlDataAdapter
        protected SqlDataReader myReader;  //MySqlDataReader

        private string host = @"USERINSTANCE\SQLEXPRESS";
        private string database = "DB_ALERTAS";
        private string user = "user";
        private string pass = "pass";

        #endregion

        #region Conecta y Desconecta

        /// <summary>
        /// Conexion automatica
        /// </summary>
        protected void Conectar()
        {
            try
            {
                conn = new SqlConnection();
                comando = new SqlCommand();
                myAdapter = new SqlDataAdapter();
                myDataSet = new DataSet();


                MyConnectionString = "Server=" + host + ";" +
                                  "Database=" + database + ";User ID=" + user + ";Password=" + pass + ";" +
                                  "Pooling=true;" +
                                  "Min Pool Size=0;" +
                                  "Max Pool Size=100;" +
                                  "Connection Lifetime=0";

                conn.ConnectionString = MyConnectionString;
                conn.Open();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Desconectar();
                this.RaiseEventObjeto(ex.Message, ex.Number);
            }
        }

        /// <summary>
        /// Conexión por parametros
        /// </summary>
        /// <param name="host"></param>
        /// <param name="db"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        protected void Conectar(string host, string db, string user, string pass)
        {
            try
            {
                conn = new SqlConnection();
                comando = new SqlCommand();
                myAdapter = new SqlDataAdapter();
                myDataSet = new DataSet();

                MyConnectionString = "Server=" + host + ";" +
                                    "Database=" + db + ";User ID=" + user + ";Password=" + pass + ";" +
                                    "Pooling=true;" +
                                    "Min Pool Size=0;" +
                                    "Max Pool Size=100;" +
                                    "Connection Lifetime=0";

                if (user == string.Empty && pass == string.Empty)
                {
                    MyConnectionString = "Server=" + host + ";Database=" + db + ";Integrated Security=SSPI";
                }

                //MyConnectionString = @"integrated security=true;initial catalog = " + Registros.bd_sqlserver + "; data source=" + Registros.host_sqlserver + "";
                conn.ConnectionString = MyConnectionString;
                conn.Open();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Desconectar();
                this.RaiseEventObjeto(ex.Message, ex.Number);
            }
        }

        /// <summary>
        /// Conexion autentificación de windows
        /// </summary>
        /// <param name="host"></param>
        /// <param name="database"></param>
        protected void Conectar(string host, string database)
        {
            try
            {
                conn = new SqlConnection();
                comando = new SqlCommand();
                myAdapter = new SqlDataAdapter();
                myDataSet = new DataSet();


                MyConnectionString = @"integrated security=true;initial catalog = " + host + "; data source=" + database + "";
                conn.ConnectionString = MyConnectionString;
                conn.Open();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Desconectar();
                this.RaiseEventObjeto(ex.Message, ex.Number);
            }
        }

        protected void Desconectar()
        {
            try
            {
                comando.Dispose();
                myAdapter.Dispose();
                conn.Close();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Desconectar();
                this.RaiseEventObjeto(ex.Message, ex.Number);
            }
        }
        /// <summary>
        /// Transforma un DataReader A un DATATABLE
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        protected DataTable GetTable(SqlDataReader _reader)
        {
            DataTable dt = new DataTable();
            dt.Load(_reader);
            return dt;
        }
        #endregion

        /// <summary>
        /// Metodo para levantar el evento ErrorObjeto
        /// </summary>
        /// <param name="descripcionError"></param>
        public void RaiseEventObjeto(string descripcionError, int numero)
        {
            if (ErrorObjeto != null)
            {
                ErrorObjeto(descripcionError, numero);
            }
        }

        public string GetConnectionString()
        {
            return MyConnectionString;
        }
    }
}
