using System;

namespace Examples.Assemblies
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"★{typeof(Program).Name}.{nameof(Main)} is Called.");
        }
    }
}
