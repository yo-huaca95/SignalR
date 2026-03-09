using Microsoft.AspNet.SignalR;
using SignalR.Conexiones.Mysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalR.Hubs
{
    public class EjecucionTareas
    {
        public static async Task Procesar(int jobId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            ConexionMysql.MarcarEnProceso(jobId);

            for (int i = 0; i <= 100; i += 10)
            {
                await Task.Delay(1000);

                ConexionMysql.ActualizarProgreso(jobId, i);

                context.Clients.All.recibirProgreso(jobId, i);
            }

            ConexionMysql.Finalizar(jobId);

            context.Clients.All.recibirMensaje("Job " + jobId + " terminado");
        }
    }
}