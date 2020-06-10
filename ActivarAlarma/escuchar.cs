using System;
using System.Data;
using System.Data.SqlClient;

namespace ActivarAlarma
{
    public class escuchar : Adaptador_sqlserver
    {

        public bool minGuardar(string rut, int sector)
        {
            string SQL = "";
            try
            {
                Conectar();
                SQL = "insert into notificaciones(fecha_hora,rut_func,sector) values(@fecha_hora,@rut_func,@sector)";

                comando.Parameters.Add("@fecha_hora", SqlDbType.DateTime).Value = DateTime.Now;
                comando.Parameters.Add("@rut_fun", SqlDbType.VarChar).Value = rut;
                comando.Parameters.Add("@sector", SqlDbType.Int).Value = sector;
                comando.CommandText = SQL;
                comando.Connection = conn;
                comando.ExecuteNonQuery();
                Desconectar();
                return true;
                //GuardaCorrecto();
            }
            catch (SqlException ex)
            {
                Desconectar();
                this.RaiseEventObjeto(ex.Message, ex.Number);
                return false;
            }
        }

    }
}
