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

            Console.WriteLine("Starting threads...");

            int rangeCount = endRange - startRange + 1;
            Thread[] threads = new Thread[rangeCount];

            for (int i = startRange; i <= endRange; i++)
            {
                int number = i; // Capture the current value of i
                threads[i - startRange] = new Thread(() => PrintNumberSequentially(number));
                threads[i - startRange].Start();
            }

            // Wait for all threads to complete
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("All threads completed.");
        }

        static void PrintNumberSequentially(int number)
        {
            lock (_lockObject)
            {
                // Ensure numbers are printed in the correct sequence
                while (_current != number - 1)
                {
                    Monitor.Wait(_lockObject);
                }

                // Print thread name (ID) and the number
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} processed number: {number}");
                _current = number;
                Monitor.PulseAll(_lockObject);
            }
        }
    }
}