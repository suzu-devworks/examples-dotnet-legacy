using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Examples.Assemblies.AppDomains
{
    class AssemblyShadowCopyExecutor
    {
        private const string _LOCAL_ASSEMBLY_PATH = @"Copies";
        private const string _CACHED_PATH = @"_cache";

        private static AppDomain CreateAppDomainWithShadowCopy(string localAssemblyPath)
        {
            var setup = new AppDomainSetup();
            //copies default. setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            setup.PrivateBinPath = localAssemblyPath;

            setup.ShadowCopyFiles = true.ToString(); //required.
            setup.CachePath = _CACHED_PATH; //required.
            setup.ApplicationName = "OtherDomainApplication";   //option.

            var domain = AppDomain.CreateDomain("OtherDomain", null, setup);

            return domain;
        }

        private static void NewDomain_DomainUnload(object sender, EventArgs e)
        {
            var domain = AppDomain.CurrentDomain;
            Console.WriteLine($"〇Unloaded: {domain.FriendlyName}");
        }

        private static void NewDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Console.WriteLine($"〇FirstChanceException: {e.Exception.Message}");
        }

        private static void NewDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"〇UnhandledException: {e.ExceptionObject}");
        }

        private static void DoCreateAppDomain()
        {
            Console.WriteLine($"■{nameof(DoCreateAppDomain)}");

            Console.WriteLine(AppDomain.CurrentDomain.Dump());

            var newDomain = CreateAppDomainWithShadowCopy(_LOCAL_ASSEMBLY_PATH);
            newDomain.DomainUnload += NewDomain_DomainUnload;
            Console.WriteLine(newDomain.Dump());

            try
            {
                Console.WriteLine($"Unloading domain is [{newDomain.FriendlyName}].");
                AppDomain.Unload(newDomain);
                Console.WriteLine($"Unloaded domain is [{newDomain.FriendlyName}].");

            }
            catch (AppDomainUnloadedException ex)
            {
                Console.WriteLine($"×{nameof(DoCreateAppDomain)} is catched: {ex.Message}.");
            }

            Console.WriteLine();
            return;
        }

        private static void DoExecuteAssembly()
        {
            Console.WriteLine($"■{nameof(DoExecuteAssembly)}");

            var assemblyName = @"Examples.Assemblies.Plugin";

            var newDomain = CreateAppDomainWithShadowCopy(_LOCAL_ASSEMBLY_PATH);

            newDomain.ExecuteAssembly($@"{_LOCAL_ASSEMBLY_PATH}\{assemblyName}.exe");

            AppDomain.Unload(newDomain);

            Console.WriteLine();
            return;
        }

        private static void OnCallebacked()
        {
            Console.WriteLine(AppDomain.CurrentDomain.Dump());

            var assemblyName = @"Examples.Assemblies.Plugin";
            var typeName = @"Examples.Assemblies.Plugins.PlugExecutor";
            var methodName = @"DoAction";

            //search PrivateBinPath.
            var asm = Assembly.Load(assemblyName);
            var pluginType = asm.GetType(typeName);
            var method = pluginType.GetMethod(methodName);

            var instance = Activator.CreateInstance(pluginType);

            method.Invoke(instance, null);

            return;
        }

        private static void DoCallBack()
        {
            Console.WriteLine($"■{nameof(DoCallBack)}");

            var newDomain = CreateAppDomainWithShadowCopy(_LOCAL_ASSEMBLY_PATH);

            newDomain.DoCallBack(OnCallebacked);

            AppDomain.Unload(newDomain);

            Console.WriteLine();
            return;
        }

        private static void ModifyAssemby(long value, string localPath)
        {
            var assemblyName = @"Examples.Assemblies.Plugin";
            var typeName = @"Examples.Assemblies.Plugins.PlugExecutor";
            var methodName = @"DoAction";

            var assemblyFilePath = Path.Combine(localPath, $"{assemblyName}.exe");
            var backupFilePath = Path.Combine(localPath, $"{assemblyName}.exe.backup");
            Console.WriteLine($"Copy to {assemblyFilePath} to backup.");

            File.Delete(backupFilePath);
            File.Move(assemblyFilePath, backupFilePath);

            using (var assemblyDef = AssemblyDefinition.ReadAssembly(backupFilePath))
            {
                var methodDef = assemblyDef.Modules
                    .SelectMany(x => x.Types)
                    .Where(x => x.FullName == typeName)
                    .SelectMany(x => x.Methods)
                    .FirstOrDefault(x => x.Name == methodName);

                //Console.WriteLine(methodDef.Dump());

                var il = methodDef.Body.GetILProcessor();
                var loadstring = methodDef.Body.Instructions.FirstOrDefault(x => x.OpCode == OpCodes.Ldstr);
                if (loadstring != null)
                {
                    il.InsertBefore(loadstring, il.Create(OpCodes.Ldc_I8, value));
                    il.InsertBefore(loadstring, il.Create(OpCodes.Stloc_0));
                }

                File.Delete(assemblyFilePath);
                assemblyDef.Write(assemblyFilePath);
            }

            return;
        }

        private static void DoGetWrap()
        {
            Console.WriteLine($"■{nameof(DoGetWrap)}");

            var assemblyName = @"Examples.Assemblies.Plugin";
            var typeName = @"Examples.Assemblies.Plugins.PlugExecutor";

            var newDomain = CreateAppDomainWithShadowCopy(_LOCAL_ASSEMBLY_PATH);

            var insrance = (IPlugable)newDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
            insrance.DoAction();

            ModifyAssemby(DateTime.Now.Ticks, _LOCAL_ASSEMBLY_PATH);

            var insrance2 = (IPlugable)newDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
            insrance2.DoAction();

            AppDomain.Unload(newDomain);

            var renewDomain = CreateAppDomainWithShadowCopy(_LOCAL_ASSEMBLY_PATH);
            var insrance3 = (IPlugable)renewDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
            insrance3.DoAction();

            AppDomain.Unload(renewDomain);

            Console.WriteLine();
            return;
        }

        private static void DoErrorHandling()
        {
            Console.WriteLine($"■{nameof(DoErrorHandling)}");

            var assemblyName = @"Examples.Assemblies.Plugin";
            var typeName = @"Examples.Assemblies.Plugins.PlugExecutor";

            var newDomain = CreateAppDomainWithShadowCopy(_LOCAL_ASSEMBLY_PATH);
            newDomain.FirstChanceException += NewDomain_FirstChanceException;
            newDomain.UnhandledException += NewDomain_UnhandledException;

            try
            {
                var insrance = (IPlugable)newDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
                insrance.ThrownAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"×{nameof(DoErrorHandling)} is catched: {ex.Message}.");
            }

            AppDomain.Unload(newDomain);

            Console.WriteLine();
            return;
        }


        internal static void Exec(string[] args)
        {
            DoCreateAppDomain();

            DoExecuteAssembly();

            DoCallBack();

            DoGetWrap();

            DoErrorHandling();

            return;
        }


    }
}
