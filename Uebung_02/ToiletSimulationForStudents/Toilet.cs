using System;
using System.Threading;

namespace VSS.ToiletSimulation
{
    public class Toilet
    {
        public string Name { get; private set; }
        public IQueue Queue { get; private set; }

        private Thread thread;
        private Toilet() { }
        public Toilet(string name, IQueue queue)
        {
            Name = name;
            Queue = queue;
        }

        public void Consume()
        {
            thread = new Thread(Run);
            thread.Start();
        }

        public void Run()
        {
            while (!Queue.IsCompleted)
            {
                IJob job;
                if (Queue.TryDequeue(out job))
                {
                    job.Process();
                }
                else
                {
                    Console.WriteLine("job==NULL");
                }

            }
        }

        public void Join()
        {
            thread.Join();
        }

    }
}
