using Ext.Net;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SignalR.Conexiones.Mysql
{
    
    public class ConexionMysql
    {
        public static string connStr = "server=localhost;database=signalr_test;user=root;password=root;";
        //public void ConectarmeMysql()
        //{
        //    using (var conn = new MySqlConnection(connStr))
        //    {
        //        conn.Open();

        //        var cmd = new MySqlCommand("INSERT INTO jobs (estado, progreso) VALUES ('Pendiente',0)", conn);
        //        cmd.ExecuteNonQuery();
        //    }
        //}

        public int InsertarJob()
        {
            int id;

            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                string sql = @"INSERT INTO Jobs
        (tipo,estado,progreso,fecha_creacion)
        VALUES
        ('Liquidacion','Pendiente',0,NOW());

        SELECT LAST_INSERT_ID();";

                MySqlCommand cmd = new MySqlCommand(sql, cn);

                cn.Open();

                id = Convert.ToInt32( cmd.ExecuteScalar());
            }

            return id;
        }

        public static void MarcarEnProceso(int jobId)
        {

            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                cn.Open();

                var cmd = new MySqlCommand(
               "UPDATE jobs SET estado='EnProceso', fecha_inicio=NOW() WHERE id=@id",
               cn);
                cmd.Parameters.AddWithValue("@id", jobId);

                cmd.ExecuteNonQuery();
            }
          
        }

        public static void ActualizarProgreso(int jobId, int progreso)
        {
            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                cn.Open();

                var cmd = new MySqlCommand(
                    "UPDATE jobs SET progreso=@prog WHERE id=@id",
                    cn);

                cmd.Parameters.AddWithValue("@prog", progreso);
                cmd.Parameters.AddWithValue("@id", jobId);

                cmd.ExecuteNonQuery();
            }
        }

        public static void Finalizar(int jobId)
        {
            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                cn.Open();

                var cmd = new MySqlCommand(
                    @"UPDATE jobs 
          SET estado='Finalizado',
              progreso=100,
              fecha_fin=NOW()
          WHERE id=@id",
                    cn);

                cmd.Parameters.AddWithValue("@id", jobId);

                cmd.ExecuteNonQuery();
            }
        }

    }

}