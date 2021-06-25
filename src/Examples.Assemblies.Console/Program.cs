using System;

namespace Examples.Assemblies
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomains.Executor.Exec(args);

            Console.ReadLine();
            return;
        }
    }
}
