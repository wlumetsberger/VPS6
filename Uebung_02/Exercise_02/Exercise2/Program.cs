using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise2
{
    class Program
    {
        static void Main(string[] args)
        {
            LimitedConnectionsExample example = new LimitedConnectionsExample();
            IList<string> downloadList = new List<string>();
            for (int i = 0; i < 30; i++)
            {
                downloadList.Add("File_" + i);
            }
            // example.DownloadFilesAsync(downloadList);
            example.DownloadFiles(downloadList);
            Console.ReadKey();
            PollingExample poll = new PollingExample();
            poll.Run();
            Console.ReadKey();
        }
    }

    class LimitedConnectionsExample
    {
        private SemaphoreSlim currentDownloadSemaphore;
        private IList<Thread> threads;

        public void DownloadFilesAsync(IEnumerable<string> urls)
        {
            currentDownloadSemaphore = new SemaphoreSlim(10);
            threads = new List<Thread>();

            foreach (var url in urls)
            {
                Thread t = new Thread(DownloadFile);
                if (threads != null)
                {
                    threads.Add(t);
                }
                t.Start(url);
            }
        }
        public void DownloadFile(object url)
        {
            currentDownloadSemaphore.Wait();
            Console.WriteLine("Start downloading: " + url);
            Thread.Sleep(100);
            Console.WriteLine("Downloaded: " + url);
            currentDownloadSemaphore.Release();
        }
        public void DownloadFiles(IEnumerable<string> urls)
        {

            Console.WriteLine("Starting downloading Files");
            this.DownloadFilesAsync(urls);
            if (threads != null)
            {
                foreach (var t in threads)
                {
                    t.Join();
                }
            }
            Console.WriteLine("Finished downloading Files");
        }
    }
    class PollingExample
    {
        private const int MAX_RESULTS = 10;
        private volatile string[] results;
        private Task[] tasks;
       

        public void Run()
        {
            results = new string[MAX_RESULTS];
            tasks = new Task[MAX_RESULTS];
            // start tasks      
            for (int i = 0; i < MAX_RESULTS; i++)
            {
                tasks[i] = new Task((s) =>
                {
                    int _i = (int)s;
                    string m = Magic(_i);
                    results[_i] = m;
                   
                }, i);

                tasks[i].Start();
            }
            Task.WaitAll(tasks);    
            // output results      
            for (int i = 0; i < MAX_RESULTS; i++) Console.WriteLine(results[i]);

        }

        private string Magic(int i)
        {
            return "magic" + i;
        }

    }
}