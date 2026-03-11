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
                    await Task.Delay(2000);
                }
            }
        }
    }
}