using System.IO;
using System.Reflection;

namespace Examples.ConsoleApp.Extensions
{
    internal static class AssemblyExtensions
    {
        public static string GetFileName(this Assembly assembly)
        {
            var filename = Path.GetFileName(assembly.Location);

            return filename;
        }
    }
}
