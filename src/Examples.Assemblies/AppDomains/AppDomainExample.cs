using System;
using System.IO;
using System.Reflection;
using Examples.Assemblies.Extensions;

namespace Examples.Assemblies.AppDomains
{
    public class AppDomainExample
    {
        private const string PLUGIN_DIRECTORY_PATH = @"Prugins";
        private const string PLUGIN_ASSEMBLY_NAME = "Examples.Assemblies.Plugin";
        private const string PLUGIN_EXECUTER_NAME = "Examples.Assemblies.Plugins.PlugExecutor";

        public static void DoCreateAppDomains()
        {
            var current = AppDomain.CurrentDomain;
            Console.WriteLine(current.Dump());

            var newDomain = AppDomain.CreateDomain("AppDomain new Domain");
            Console.WriteLine(newDomain.Dump());

            var setupDomain = new AppDomainSetup()
                                .SetConfigurationFile(@"hogehoge.config")
                                .Build("Setup Domain");
            Console.WriteLine(setupDomain.Dump());

            AppDomain.Unload(newDomain);
            AppDomain.Unload(setupDomain);

            return;
        }

        public static void DoUnloadAppDomains()
        {
            var newDomain = AppDomain.CreateDomain("AppDomain new Domain");
            Console.WriteLine(newDomain.Dump());

            newDomain.DomainUnload += AppDomainEventHandlers.OnDomainUnload;

            try
            {
                Console.WriteLine($"Unloading domain is [{newDomain.FriendlyName}].");
                AppDomain.Unload(newDomain);
                Console.WriteLine($"Unloaded domain is [{newDomain.FriendlyName}]."); // throw.
            }
            catch (AppDomainUnloadedException ex)
            {
                Console.Error.WriteLine($"×{nameof(DoCreateAppDomains)} is catched: {ex.Message}.");
            }

            return;
        }

        public static void DoExecuteAssembly()
        {
            var setupDomain = new AppDomainSetup()
                                .SetPrivateBinPath(PLUGIN_DIRECTORY_PATH)
                                .Build("ExecuteAssembly Domain");
            Console.WriteLine(setupDomain.Dump());

            setupDomain.AssemblyLoad += AppDomainEventHandlers.OnAssemblyLoad;
            setupDomain.DomainUnload += AppDomainEventHandlers.OnDomainUnload;

            setupDomain.ExecuteAssemblyByName(PLUGIN_ASSEMBLY_NAME);

            var filePath = Path.Combine(setupDomain.BaseDirectory, PLUGIN_DIRECTORY_PATH, $"{ PLUGIN_ASSEMBLY_NAME}.exe");
            setupDomain.ExecuteAssembly(filePath);

            AppDomain.Unload(setupDomain);

            return;
        }

        public static void DoDoCallBack()
        {
            var setupDomain = new AppDomainSetup()
                                .SetPrivateBinPath(PLUGIN_DIRECTORY_PATH)
                                .Build("DoCallBack Domain");
            Console.WriteLine(setupDomain.Dump());

            setupDomain.AssemblyLoad += AppDomainEventHandlers.OnAssemblyLoad;
            setupDomain.DomainUnload += AppDomainEventHandlers.OnDomainUnload;

            setupDomain.DoCallBack(() =>
            {
                var domain = AppDomain.CurrentDomain;
                Console.WriteLine($"--> DoCallBack[{domain.FriendlyName}].");

                //search PrivateBinPath.
                var asm = Assembly.Load(PLUGIN_ASSEMBLY_NAME);
                var pluginType = asm.GetType(PLUGIN_EXECUTER_NAME);
                var method = pluginType.GetMethod(@"DoAction");

                var instance = Activator.CreateInstance(pluginType);

                method.Invoke(instance, null);
                return;
            });

            AppDomain.Unload(setupDomain);

            return;
        }

        public static void DoCreateInstance()
        {
            var setupDomain = new AppDomainSetup()
                                .SetPrivateBinPath(PLUGIN_DIRECTORY_PATH)
                                .Build("CreateInstance Domain");
            Console.WriteLine(setupDomain.Dump());

            setupDomain.AssemblyLoad += AppDomainEventHandlers.OnAssemblyLoad;
            setupDomain.DomainUnload += AppDomainEventHandlers.OnDomainUnload;

            var insrance = (IPlugable)setupDomain
                .CreateInstanceAndUnwrap(PLUGIN_ASSEMBLY_NAME, PLUGIN_EXECUTER_NAME);
            insrance.DoAction();

            var handle = setupDomain.CreateInstance(PLUGIN_ASSEMBLY_NAME, PLUGIN_EXECUTER_NAME);
            var insrance2 = (IPlugable)handle.Unwrap();
            insrance2.DoAction();

            AppDomain.Unload(setupDomain);

            return;
        }

        public static void DoHandleException()
        {
            var setupDomain = new AppDomainSetup()
                                .SetPrivateBinPath(PLUGIN_DIRECTORY_PATH)
                                .Build("ThrownAction Domain");
            Console.WriteLine(setupDomain.Dump());

            setupDomain.AssemblyLoad += AppDomainEventHandlers.OnAssemblyLoad;
            setupDomain.DomainUnload += AppDomainEventHandlers.OnDomainUnload;
            setupDomain.FirstChanceException += AppDomainEventHandlers.OnFirstChanceException;
            setupDomain.UnhandledException += AppDomainEventHandlers.OnUnhandledException;

            try
            {
                var insrance = (IPlugable)setupDomain
                    .CreateInstanceAndUnwrap(PLUGIN_ASSEMBLY_NAME, PLUGIN_EXECUTER_NAME);
                insrance.ThrownAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"×{nameof(DoHandleException)} is catched: {ex.Message}.");
            }

            AppDomain.Unload(setupDomain);

            return;
        }

    }

}
