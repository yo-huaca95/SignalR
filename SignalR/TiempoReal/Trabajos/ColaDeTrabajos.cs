using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SignalR.TiempoReal.Trabajos
{
    public class ColaDeTrabajos
    {
        public static ConcurrentQueue<int> Cola = new ConcurrentQueue<int>();
        private static readonly HashSet<int> TareasEnCola = new HashSet<int>();
        private static readonly object Locker = new object();


        public static void Enqueue(int tareaId)
        {

            lock (Locker)
            {
                if (!TareasEnCola.Contains(tareaId))
                {
                    Cola.Enqueue(tareaId);
                    TareasEnCola.Add(tareaId);
                }
            }
        }

        public static bool TryDequeue(out int tareaId)
        {

            if (Cola.TryDequeue(out tareaId))
            {
                lock (Locker)
                {
                    TareasEnCola.Remove(tareaId);
                }
                return true;

            }
            return false;
        }

        public static bool EstaEnCola(int tareaId)
        {
            lock (Locker)
            {
                return TareasEnCola.Contains(tareaId);
            }
        }

    }
}