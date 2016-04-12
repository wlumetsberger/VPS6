using System;
using System.Linq;
using System.Threading;

namespace VSS.ToiletSimulation
{

    public class FIFOQueue : Queue
    {
        private readonly SemaphoreSlim semaphore;

        public FIFOQueue() {
            semaphore = new SemaphoreSlim(0);
        }

        public override void Enqueue(IJob job)
        {
            if (addingComplete)
            {
                throw new InvalidOperationException("Queue is already complete");
            }
            lock (queueLock)
            {
                queue.Add(job);
            }
            semaphore.Release();
        }

        public override bool TryDequeue(out IJob job)
        {
            job = null;
            if (IsCompleted)
            {
                return false;
            }
            semaphore.Wait();
            lock (queueLock)
            {
                if (!IsCompleted)
            {
                
                    job = GetJob();
                    queue.Remove(job);
                
                return true;
            }
            }
            return false;
        }

        protected virtual  IJob GetJob()
        {
            lock (queueLock)
            {
                return queue.First();
            }

        }

        public override void CompleteAdding()
        {
            base.CompleteAdding();
        }
    }
}
