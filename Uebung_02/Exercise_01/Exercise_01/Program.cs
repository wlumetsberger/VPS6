using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise_01
{
    class Program
    {
        public static object lockingObject = new object();
        static void Main(string[] args)
        {
            /*Exercise e = new Exercise();
            Thread t1 = new Thread(() => e.increaseForValueTimes(1, 100000000));
            Thread t2 = new Thread(() => e.increaseForValueTimes(1, 100000000));
            Thread t3 = new Thread(() => e.increaseForValueTimes(1, 100000000));
            Console.WriteLine("Value before starting threads is:" + e.getValue());
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine("Value before starting threads is:" + e.getValue() + " and should be " + (3 * 100000000));*/
            RaceConditionExample example = new RaceConditionExample();
            example.Run();
            Console.ReadKey();
        }
    }

    class Exercise
    {
        private int x = 0;
        public void increaseForValueTimes(int value, int times)
        {
            for (int i = 0; i < times; i++)
            {
                lock (Program.lockingObject)
                {
                    x = x + value;
                }
            }
        }

        public int getValue()
        {
            return x;
        }
    }

    public class RaceConditionExample
    {
        private const int N = 1000;
        private const int BUFFER_SIZE = 10;
        private double[] buffer;


        private SemaphoreSlim readerSemaphore;
        private SemaphoreSlim writerSemaphore;

        public void Run()
        {
            buffer = new double[BUFFER_SIZE];
            readerSemaphore = new SemaphoreSlim(0);
            writerSemaphore = new SemaphoreSlim(BUFFER_SIZE);

            var t1 = new Thread(Reader);
            var t2 = new Thread(Writer);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
        }
        void Reader()
        {
            var readerIndex = 0;
            for (int i = 0; i < N; i++)
            {
                readerSemaphore.Wait();
                Console.WriteLine(buffer[readerIndex]);
                readerIndex = (readerIndex + 1) % BUFFER_SIZE;
                writerSemaphore.Release();

            }
        }
        void Writer()
        {
            var writerIndex = 0;
            for (int i = 0; i < N; i++)
            {
                writerSemaphore.Wait();
                buffer[writerIndex] = (double)i;
                writerIndex = (writerIndex + 1) % BUFFER_SIZE;
                readerSemaphore.Release();

            }
            
        }
    }
}

