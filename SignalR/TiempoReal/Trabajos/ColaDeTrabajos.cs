using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SignalR.TiempoReal.Trabajos
{
    public class ColaDeTrabajos
    {
        public static ConcurrentQueue<int> Cola = new ConcurrentQueue<int>();
        private static readonly HashSet<int> TareasEnCola = new HashSet<int>();
        private static readonly object Candado= new object();


        public static void Encolar(int tareaId)
        {

            lock (Candado)
            {
                if (!TareasEnCola.Contains(tareaId))
                {
                    Cola.Enqueue(tareaId);
                    TareasEnCola.Add(tareaId);
                }
            }
        }

        public static bool IntentarDesencolar(out int tareaId)
        {

            if (Cola.TryDequeue(out tareaId))
            {
                lock (Candado)
                {
                    TareasEnCola.Remove(tareaId);
                }
                return true;

            }
            return false;
        }

        public static bool EstaEnCola(int tareaId)
        {
            lock (Candado)
            {
                return TareasEnCola.Contains(tareaId);
            }
            
        }

    }
}