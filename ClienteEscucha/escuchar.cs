using System;
using System.Data;
using System.Data.SqlClient;

namespace ClienteEscucha
{
    public class escuchar : Adaptador_sqlserver
    {
        public DataTable msoBuscarNotificacion()
        {
            DataTable dt = null;

            try
            {
                Conectar();
                string SQL;
                SQL = "SELECT top 1 max(id) as id, sector, rut_func from notificaciones group by rut_func, sector order by id desc"; 
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

        public int msoObtenerUltimaNotificacion()
        {
            DataTable dt = null;
            int ultima_notificacion = 0;
            try
            {
                Conectar();
                string SQL;
                SQL = "SELECT max(id) as id from notificaciones";
                comando = conn.CreateCommand();
                comando.CommandText = SQL;
                myReader = comando.ExecuteReader();
                dt = GetTable(myReader);
                Desconectar();
                if(dt!=null)
                {
                    if(dt.Rows.Count > 0)
                    {
                        if(dt.Rows[0]["id"].ToString()!="")
                        {
                            ultima_notificacion = int.Parse(dt.Rows[0]["id"].ToString());
                        }
                    }
                    else
                    {
                        ultima_notificacion = 0;
                    }
                }
                return ultima_notificacion;
            }
            catch (SqlException ex)
            {
                Desconectar();
                this.RaiseEventObjeto(ex.Message, ex.Number);
            }
            return ultima_notificacion;
        }
    }
}
