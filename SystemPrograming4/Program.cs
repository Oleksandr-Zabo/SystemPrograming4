using System;
using System.Threading.Tasks;

namespace SystemPrograming4;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting thread...");
        await Task.Run(() =>
        {
            for (int i = 0; i <= 50; i++)
            {
                Console.WriteLine(i);
            }
        });
        Console.WriteLine("Thread completed.");
    }
}

