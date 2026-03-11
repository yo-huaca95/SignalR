using Microsoft.AspNet.SignalR;
using SignalR.Conexiones.Mysql;
using SignalR.TiempoReal.Concentradores;
using System.Threading.Tasks;

namespace SignalR.TiempoReal.Trabajos
{
    public class ProcesadorDeTareas
    {
        public static async Task Procesar(int tareaId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ConcentradorDeTareas>();
            
            ConexionMysql.MarcarEnProceso(tareaId);
            context.Clients.All.actualizarEstado(tareaId, "Generando");
            for (int i = 0; i <= 100; i += 10)
            {
                await Task.Delay(1000);

                ConexionMysql.ActualizarProgreso(tareaId, i);

                context.Clients.All.recibirProgreso(tareaId, i);
            }

            ConexionMysql.Finalizar(tareaId);
            context.Clients.All.actualizarEstado(tareaId, "Terminado");

            //context.Clients.All.recibirMensaje("Tarea Numero: " + jobId + " terminado");
        }
    }
}