using System;
using System.Threading;

namespace SystemPrograming4
{
    class Program
    {
        private static object _lockObject = new object();
        private static int _current = 0;

        static void Main(string[] args)
        {
            Console.Write("Enter start of range: ");
            int startRange = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter end of range: ");
            int endRange = Convert.ToInt32(Console.ReadLine());

            if (startRange > endRange)
            {
                (startRange, endRange) = (endRange, startRange);
            }

            Console.Write("Enter number of threads: ");
            int threadCount = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Starting threads...");

            int rangePerThread = (endRange - startRange + 1) / threadCount;
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int threadStart = startRange + i * rangePerThread;
                int threadEnd = (i == threadCount - 1) ? endRange : threadStart + rangePerThread - 1;

                threads[i] = new Thread(() => PrintRange(threadStart, threadEnd));
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("All threads completed.");
        }

        static void PrintRange(int start, int end)
        {
            lock (_lockObject)
            {
                for (int i = start; i <= end; i++)
                {
                    // Ensure the output is in sequence
                    while (_current != i - 1)
                    {
                        Monitor.Wait(_lockObject);
                    }

                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} processed number: {i}");
                    _current = i;
                    Monitor.PulseAll(_lockObject);
                }
            }
        }
    }
}
