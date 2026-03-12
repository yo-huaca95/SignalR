using Ext.Net;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SignalR.Conexiones.Mysql
{
    
    public class ConexionMysql
    {
        public static string connStr = "server=localhost;database=signalr_test;user=root;password=root;";
        public DataTable ObtenerTareas()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                string sql = @"SELECT 
                        id,
                        tipo,
                        estado,
                         CAST(progreso / 100 AS DECIMAL(5,1)) as progreso,
                        fecha_creacion,
                        fecha_inicio,
                        fecha_fin 
                       FROM jobs
                       ORDER BY fecha_creacion DESC";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cn.Open();

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public DataTable ObtenerTareasUsuario(string usuario)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                string sql = @"
                SELECT 
                id,
                usuario,
                tipo,
                estado,
                CAST(progreso / 100 AS DECIMAL(5,1)) as progreso,
                payload,
                fecha_creacion,
                fecha_inicio,
                fecha_fin
                FROM jobs
                WHERE usuario = @usuario";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);

                    cn.Open();

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public DataTable ObtenerTareasPendientesUsuario(string usuario)
        {
            DataTable dt = new DataTable();

            using (var cn = new MySqlConnection(connStr))
            {
                string sql = @"
                SELECT 
                    id,
                    usuario,
                    tipo,
                    estado,
                    progreso,
                    fecha_creacion
                FROM jobs
                WHERE estado = 'Pendiente'
                AND usuario = @usuario
                ORDER BY fecha_creacion ASC";

                using (var cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cn.Open();

                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public static DataTable ObtenerTareasPendientes()
        {
            DataTable dt = new DataTable();

            using (var cn = new MySqlConnection(connStr))
            {
                string sql = @"
                SELECT 
                    id,
                    usuario,
                    tipo,
                    estado,
                    progreso,
                    fecha_creacion
                FROM jobs
                WHERE estado = 'Pendiente'
                ORDER BY fecha_creacion ASC";

                using (var cmd = new MySqlCommand(sql, cn))
                {
                    cn.Open();

                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public static DataTable ObtenerTarea(int idTarea)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                string sql = @"
                SELECT 
                id,
                usuario,
                tipo,
                estado,
                progreso,
                payload,
                fecha_creacion,
                fecha_inicio,
                fecha_fin
                FROM jobs
                WHERE id = @idTarea
                LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@idTarea", idTarea);

                    cn.Open();

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public int InsertarJob(string usuario, string payloadJson)
        {
            int id;

            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                string sql = @"
            INSERT INTO Jobs
            (tipo, estado, progreso, fecha_creacion, usuario, payload)
            VALUES
            (@tipo, @estado, 0, NOW(), @usuario, @payload);

            SELECT LAST_INSERT_ID();";

                MySqlCommand cmd = new MySqlCommand(sql, cn);

                cmd.Parameters.AddWithValue("@tipo", "Liquidacion");
                cmd.Parameters.AddWithValue("@estado", "EnCola");
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@payload", payloadJson);

                cn.Open();

                id = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return id;
        }


        public static void MarcarEnCola(int idTarea)
        {
            using (var cn = new MySqlConnection(connStr))
            {
                string sql = @"
            UPDATE jobs 
            SET estado = 'Pendiente'
            WHERE id = @id";

                using (var cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", idTarea);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void MarcarEnProceso(int jobId)
        {

            using (MySqlConnection cn = new MySqlConnection(connStr))
            {
                cn.Open();

                var cmd = new MySqlCommand(
               "UPDATE jobs SET estado='Generando', fecha_inicio=NOW() WHERE id=@id",
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
                      SET estado='Terminado',
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