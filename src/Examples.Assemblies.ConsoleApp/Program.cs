using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Examples.Assemblies.Extensions;
using Mono.Options;

namespace Examples.Assemblies
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var exename = Assembly.GetEntryAssembly().GetFileName();
            var showVersion = false;
            var showHelp = false;

            var suite = new CommandSet(exename)
            {
                $"usage: {exename} [--version] ... <command> [<args>]",
                "",
                "Common Options:",
                { "version", "show version info", v => showVersion = v != null },
                { "help", "show this message and exit",  v => showHelp = v != null },
                "",
                "Example commands:",
                //new Command("version") { Run = v =>
                //{
                //    var version = Assembly.GetEntryAssembly().GetName().Version;
                //    Console.WriteLine(version);
                //} },
                LoadCommands(),
            };

            suite.Run(args);

            return;
        }

        private static IEnumerable<Command> LoadCommands()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Command)))
                .Select(t => (Command)Activator.CreateInstance(t));
        }

    }
}
