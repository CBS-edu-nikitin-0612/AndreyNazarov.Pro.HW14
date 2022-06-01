using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task4
{
    internal class Program
    {
        private static int _counter = 0;

        private static readonly object _block = new object();

        private static void Function()
        {
            lock (_block)
            {
                for (int i = 0; i < 10; ++i)
                {
                    Thread.Sleep(20);
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - {++_counter}");
                }
            }
        }

        private static async Task FunctionAsync()
        {
            await Task.Run(Function);
        }

        static void Main()
        {
            Task[] tasks = { FunctionAsync(), FunctionAsync(), FunctionAsync() };
            Task.WaitAll(tasks);
            Console.ReadKey();
        }
    }

}
