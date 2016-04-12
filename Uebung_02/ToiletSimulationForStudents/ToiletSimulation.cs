using System;

namespace VSS.ToiletSimulation
{
    public class ToiletSimulation
    {
        public static void Main()
        {
            int randomSeed = new Random().Next();
            var q = new ToiletQueue(); //new NetFIFOQueue();
            TestQueue(q, randomSeed);
        }

        public static void TestQueue(IQueue queue, int randomSeed)
        {
            Random random = new Random(randomSeed);

            PeopleGenerator[] producers = new PeopleGenerator[Parameters.Producers];
            for (int i = 0; i < producers.Length; i++)
                producers[i] = new PeopleGenerator("People Generator " + i, queue, random.Next());

            Toilet[] consumers = new Toilet[Parameters.Consumers];
            for (int i = 0; i < consumers.Length; i++)
                consumers[i] = new Toilet("Toilet " + i, queue);

            Console.WriteLine("Testing " + queue.GetType().Name + ":");

            Analysis.Reset();
            for (int i = 0; i < producers.Length; i++)
                producers[i].Produce();
            for (int i = 0; i < consumers.Length; i++)
                consumers[i].Consume();

            for (int i = 0; i < consumers.Length; i++)
            {
                consumers[i].Join();
            }


            Analysis.Display();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.Read();
        }

    }
}
