using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace SystemPrograming4
{
    class Program
    {
        private static short[] _numbers;
        private static short _maximum;
        private static short _minimum;
        private static double _average;

        static void Main(string[] args)
        {
            Console.WriteLine("Generating random numbers...");

            // Generate 10,000 random numbers
            _numbers = GenerateNumbers(10000);

            // Create threads for calculations
            Thread maxThread = new Thread(() => _maximum = _numbers.Max());
            Thread minThread = new Thread(() => _minimum = _numbers.Min());
            Thread avgThread = new Thread(() => _average = (double)Sum(_numbers) / _numbers.Length);
            Thread writeThread = new Thread(() => WriteToFile("output.txt"));

            // Start threads
            maxThread.Start();
            minThread.Start();
            avgThread.Start();

            // Wait for calculation threads to complete
            maxThread.Join();
            minThread.Join();
            avgThread.Join();

            Console.WriteLine($"Maximum: {_maximum}");
            Console.WriteLine($"Minimum: {_minimum}");
            Console.WriteLine($"Average: {_average:F2}");

            // Start writing to file thread
            writeThread.Start();
            writeThread.Join();

            Console.WriteLine("Results written to file.");
        }

        static short[] GenerateNumbers(int count)
        {
            Random random = new Random();
            short[] generateNumbers = new short[count];
            for (int i = 0; i < count; i++)
            {
                generateNumbers[i] = (short)random.Next(1, 10001); // Random numbers between 1 and 10,000
            }
            return generateNumbers;
        }
        
        static long Sum(short[] numbers)
        {
            long sum = 0;
            foreach (short num in numbers)
            {
                sum += num;
            }
            return sum;
        }


        static void WriteToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Numbers:");
                foreach (short num in _numbers)
                {
                    writer.WriteLine(num);
                }
                writer.WriteLine();
                writer.WriteLine($"Maximum: {_maximum}");
                writer.WriteLine($"Minimum: {_minimum}");
                writer.WriteLine($"Average: {_average:F2}");
            }
        }
    }
}
