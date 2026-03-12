using Microsoft.AspNet.SignalR;
using SignalR.Conexiones.Mysql;
using SignalR.TiempoReal.Concentradores;
using System;
using System.Threading.Tasks;

namespace SignalR.TiempoReal.Trabajos
{
    public class ProcesadorDeTareas
    {
        public static async Task Procesar(int tareaId)
        {
            var contexto = GlobalHost.ConnectionManager.GetHubContext<ConcentradorDeTareas>();

            try { 
            // 1. Obtener la info de la tarea DESDE BD
            var tarea = ConexionMysql.ObtenerTarea(tareaId); 

            if (tarea.Rows.Count == 0)
                return; // o lanzar excepción

            var fila = tarea.Rows[0];

            string usuario = fila["usuario"].ToString();
            //string payloadJson = fila["payload"].ToString();


            // 2. Marcar en proceso
            ConexionMysql.MarcarEnProceso(tareaId);
            contexto.Clients.Groups(new[] { usuario, "Administrador" }).actualizarEstado(tareaId, "Generando");

            // 3. Ejecutar el procedimiento almacenado que procesa todos los empleados
            //ConexionMysql.EjecutarProcedimientoLiquidacionBase(idTarea);



            // 4. Consultar avance y notificar solo a ese usuario
            /*   bool finalizado = false;
               while (!finalizado)
               {
                   var detalle = ConexionMysql.ObtenerDetalleProgreso(idTarea);

                   contexto.Clients.Group(usuario).actualizarDetalle(idTarea, detalle);

                   finalizado = ConexionMysql.TareaFinalizada(idTarea);

                   await Task.Delay(1000);
               }*/ //Aplicar a ala logica


            for (int i = 0; i <= 100; i += 10)
            {
                await Task.Delay(1000);

                ConexionMysql.ActualizarProgreso(tareaId, i);

                contexto.Clients.Groups(new[] { usuario, "Administrador" }).recibirProgreso(tareaId, i);
            }

            ConexionMysql.Finalizar(tareaId);
            contexto.Clients.Groups(new[] { usuario, "Administrador" }).actualizarEstado(tareaId, "Terminado");

                //context.Clients.All.recibirMensaje("Tarea Numero: " + jobId + " terminado");


            }
            catch (Exception ex)
                {
                            // 6. Manejo de error
                            //ConexionMysql.MarcarEnError(tareaId, ex.Message);

                            contexto.Clients.Groups(new[] {"Administrador" })
                                .actualizarEstado(tareaId, "Error");
                 }

        }
    }
}