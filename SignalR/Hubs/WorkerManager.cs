using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalR.Hubs
{
    public class WorkerManager
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
                if (JobQueue.TryDequeue(out int jobId))
                {
                    await EjecucionTareas.Procesar(jobId);
                }
                else
                {
                    await Task.Delay(2000);
                }
            }
        }
    }
}