using System.Collections.Generic;
using Mono.Options;

namespace Examples.Assemblies.Extensions
{
    internal static class OptionsExtensions
    {
        public static void Add(this CommandSet suite, IEnumerable<Command> commands)
        {
            foreach (var command in commands)
            {
                suite.Add(command);
            }

            return;
        }
    }
}
