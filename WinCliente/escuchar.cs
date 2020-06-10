using System;
using System.Data;
using System.Data.SqlClient;

namespace WinCliente
{
    public class escuchar : Adaptador_sqlserver
    {

        public DataTable msoObtenerUltimaNotificacion()
        {
            DataTable dt = null;
            try
            {
                Conectar();
                string SQL;
                SQL = "SELECT n.*, f.nombre_completo from notificaciones n " +
                    "inner join func f on f.rut_func = n.rut_func " +
                    "where id = (select max(id) from notificaciones)";
                comando = conn.CreateCommand();
                comando.CommandText = SQL;
                myReader = comando.ExecuteReader();
                dt = GetTable(myReader);
                Desconectar();
                return dt;
            }
            catch (SqlException ex)
            {
                Desconectar();
                this.RaiseEventObjeto(ex.Message, ex.Number);
            }
            return dt;
        }
    }
}
