using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncConsoleApp
{
    class Program
    {
        static readonly object consoleLock = new object();

        static void PrintNumber(ConsoleColor color, int number)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = color;
                Console.Write(number + " ");
                Console.ResetColor();
            }
        }

        static async Task<int> printC(ConsoleColor color, int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                PrintNumber(color, i);
                await Task.Delay(50);
            }

            int numOfIterations = end - start;
            return numOfIterations;
        }

        static void PrintWithThread(ConsoleColor color, int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                PrintNumber(color, i);
                Thread.Sleep(50);
            }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any key to start printing numbers in different colors...");
            Console.ReadKey(true);

            Console.WriteLine("Printing numbers in different colors (Tasks)...");

            Task<int> task1 = printC(ConsoleColor.Red, 1, 100);
            Task<int> task2 = printC(ConsoleColor.Green, 1, 100);

            int result1 = await task1;
            int result2 = await task2;

            Console.WriteLine();
            Console.WriteLine("All tasks completed.");
            Console.WriteLine();

            Console.WriteLine("Printing numbers in different colors (Threads)...");

            Thread t1 = new Thread(() => PrintWithThread(ConsoleColor.Red, 1, 100));
            Thread t2 = new Thread(() => PrintWithThread(ConsoleColor.Green, 1, 100));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine();
            Console.WriteLine("All threads completed.");
        }
    }
}
