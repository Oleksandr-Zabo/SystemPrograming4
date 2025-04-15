using System;
using System.Threading.Tasks;

namespace SystemPrograming4;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Enter start of range: ");
        int startRange = Convert.ToInt32(Console.ReadLine());
        
        Console.Write("Enter end of range: ");
        int endRange = Convert.ToInt32(Console.ReadLine());
        
        if(startRange> endRange)
        {
            (startRange, endRange) = (endRange, startRange);
        }
        
        Console.WriteLine("Starting thread...");
        await Task.Run(() =>
        {
            for (int i = startRange; i <= endRange; i++)
            {
                Console.WriteLine(i);
            }
        });
        Console.WriteLine("Thread completed.");
    }
}

