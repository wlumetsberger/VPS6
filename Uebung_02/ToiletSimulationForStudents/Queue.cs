using System;
using System.Collections.Generic;
using System.Threading;

namespace VSS.ToiletSimulation
{
    public abstract class Queue : IQueue
    {
        protected IList<IJob> queue;
        protected bool addingComplete;
        protected object queueLock = new object();
        private int complete;

        public int Count
        {
            get { return queue.Count; }
        }

        protected Queue()
        {
            queue = new List<IJob>();
        }

        public abstract void Enqueue(IJob job);


        public abstract bool TryDequeue(out IJob job);


        public virtual void CompleteAdding()
        {
            Interlocked.Increment(ref complete);
            if(complete == Parameters.Producers)
            {
                addingComplete = true;
            }
        }

        public bool IsCompleted
        {
            get
            {
               return addingComplete && Count == 0;
            }
        }
    }
}
