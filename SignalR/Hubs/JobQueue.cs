using System.Collections.Concurrent;

namespace SignalR.Hubs
{
    public class JobQueue
    {
        

        public static ConcurrentQueue<int> Cola = new ConcurrentQueue<int>();

        public static void Enqueue(int jobId)
        {
            Cola.Enqueue(jobId);
        }

        public static bool TryDequeue(out int jobId)
        {
            return Cola.TryDequeue(out jobId);
        }
    }
}