using System.Collections.Concurrent;

namespace SignalR.TiempoReal.Trabajos
{
    public class ColaDeTrabajos
    {
        

        public static ConcurrentQueue<int> Cola = new ConcurrentQueue<int>();

        public static void Enqueue(int tareaId)
        {
            Cola.Enqueue(tareaId);
        }

        public static bool TryDequeue(out int tareaId)
        {
            return Cola.TryDequeue(out tareaId);
        }
    }
}