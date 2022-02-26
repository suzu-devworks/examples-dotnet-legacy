using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Examples.Assemblies.Utility
{
    internal class PluginEditor
    {
        private const string PLUGIN_ASSEMBLY_NAME = "Examples.Assemblies.Plugin";
        private const string PLUGIN_EXECUTER_NAME = "Examples.Assemblies.Plugins.PlugExecutor";

        public static void ModifyDoAction(long value, string assemblyPath)
        {
            var tempfilePath = Path.GetTempFileName();

            using (var assemblyDef = AssemblyDefinition.ReadAssembly(assemblyPath))
            {
                var methodDef = assemblyDef.Modules
                 .SelectMany(x => x.Types)
                 .Where(x => x.FullName == PLUGIN_EXECUTER_NAME)
                 .SelectMany(x => x.Methods)
                 .FirstOrDefault(x => x.Name == nameof(IPlugable.DoAction));

                Console.WriteLine(Dump(methodDef));

                var il = methodDef.Body.GetILProcessor();
                var loadstring = methodDef.Body.Instructions.FirstOrDefault(x => x.OpCode == OpCodes.Ldstr);
                if (loadstring != null)
                {
                    il.InsertBefore(loadstring, il.Create(OpCodes.Ldc_I8, value));
                    il.InsertBefore(loadstring, il.Create(OpCodes.Stloc_0));
                }

                assemblyDef.Write(tempfilePath);

            }

            File.Delete(assemblyPath);
            File.Move(tempfilePath, assemblyPath);

            return;
        }

        private static string Dump(MethodDefinition methos)
        {
            return string.Join(Environment.NewLine, methos.Body.Instructions);
        }

    }
}
