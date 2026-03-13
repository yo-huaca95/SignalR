using SignalR.Conexiones.Mysql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SignalR.TiempoReal.Trabajos
{
    public class GestorDeTrabajadores
    {
        private static int MAX_WORKERS = 5;

        public static void Iniciar()
        {
            for (int i = 0; i < MAX_WORKERS; i++)
            {
                Task.Run(() => WorkerLoop());
            }
        }

        private static async Task WorkerLoop()
        {
            while (true)
            {
                if (ColaDeTrabajos.TryDequeue(out int tareaId))
                {
                    await ProcesadorDeTareas.Procesar(tareaId);

                }
                else
                {
                    // 2. Si la cola está vacía, buscar trabajos pendientes en BD
                    var tareasPendientes = ConexionMysql.ObtenerTareasPendientes();

                    foreach (DataRow fila in tareasPendientes.Rows)
                    {
                        int idTarea = Convert.ToInt32(fila["id"]);

                        // 2.1 Verificar si ya está en la cola (en memoria)
                        if (!ColaDeTrabajos.EstaEnCola(idTarea))
                        {
                            ConexionMysql.MarcarEnCola(idTarea);
                            ColaDeTrabajos.Enqueue(idTarea);
                        }
                    }

                    // Esperar un poco antes de revisar de nuevo
                    await Task.Delay(4000);
                }
            }
        }
    }
}