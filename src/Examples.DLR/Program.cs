using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;

namespace Examples.DLR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var types = LoadVerbOptions();
            Parser.Default.ParseArguments(args, types)
                .WithNotParsed(errors => { foreach (var e in errors) Debug.WriteLine($"Error: {e}"); })
                .WithParsedAsync(RunAsync).ConfigureAwait(false).GetAwaiter().GetResult()
                ;

            return;
        }

        private static Type[] LoadVerbOptions()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
        }

        static Task RunAsync(object options)
        {
            dynamic invoker = options;
            Task invokationTask = invoker.RunAsync();

            return invokationTask;
        }

    }

}

