using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSS.ToiletSimulation
{
    public class ToiletQueue : FIFOQueue
    {
        protected override IJob GetJob()
        {
            IJob job;
            lock (queueLock)
            {
                job = queue.OrderByDescending(q => q.DueDate).First();
            }
            return job;
        }

    }
}
