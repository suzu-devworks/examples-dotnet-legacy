using System.IO;
using System.Reflection;

namespace Examples.Assemblies.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetFileName(this Assembly assembly)
        {
            var filename = Path.GetFileName(assembly.Location);

            return filename;
        }
    }
}
