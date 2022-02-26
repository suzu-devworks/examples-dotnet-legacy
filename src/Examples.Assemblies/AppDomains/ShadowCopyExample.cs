using System;
using System.IO;
using Examples.Assemblies.Extensions;
using Examples.Assemblies.Utility;

namespace Examples.Assemblies.AppDomains
{
    public class ShadowCopyExample
    {
        private const string PLUGIN_DIRECTORY_PATH = @"Prugins";
        private const string PLUGIN_ASSEMBLY_NAME = "Examples.Assemblies.Plugin";
        private const string PLUGIN_EXECUTER_NAME = "Examples.Assemblies.Plugins.PlugExecutor";

        private const string COPY_DIRECTORY_PATH = @"_cache";

        public static void DoCreateShadowCopyInstance()
        {
            var shadowCopyDomain = new AppDomainSetup()
                                    .SetPrivateBinPath(PLUGIN_DIRECTORY_PATH)
                                    .SetShadowCopy(PLUGIN_DIRECTORY_PATH, COPY_DIRECTORY_PATH)
                                    .Build("Shadow Copy Domain");
            Console.WriteLine(shadowCopyDomain.Dump());

            shadowCopyDomain.AssemblyLoad += AppDomainEventHandlers.OnAssemblyLoad;
            shadowCopyDomain.DomainUnload += AppDomainEventHandlers.OnDomainUnload;

            var originalPath = Path.Combine(shadowCopyDomain.BaseDirectory, PLUGIN_DIRECTORY_PATH, $"{ PLUGIN_ASSEMBLY_NAME}.exe");
            var backupPath = $@"{originalPath}.back";

            //preload
            _ = shadowCopyDomain.CreateInstanceAndUnwrap(PLUGIN_ASSEMBLY_NAME, PLUGIN_EXECUTER_NAME);

            File.Delete(backupPath);
            File.Move(originalPath, backupPath);
            Console.WriteLine($"  <move> \"{originalPath}\" => \"{backupPath}\"");

            var insrance = (IPlugable)shadowCopyDomain
                .CreateInstanceAndUnwrap(PLUGIN_ASSEMBLY_NAME, PLUGIN_EXECUTER_NAME);
            insrance.DoAction();

            File.Move(backupPath, originalPath);
            Console.WriteLine($"  <restore> \"{backupPath}\" => \"{originalPath}\"");

            AppDomain.Unload(shadowCopyDomain);
        }

        public static void DoReloadDomain()
        {
            var shadowCopyDomain = new AppDomainSetup()
                                    .SetPrivateBinPath(PLUGIN_DIRECTORY_PATH)
                                    .SetShadowCopy(PLUGIN_DIRECTORY_PATH, COPY_DIRECTORY_PATH)
                                    .Build("Shadow Copy Domain");
            Console.WriteLine(shadowCopyDomain.Dump());

            shadowCopyDomain.AssemblyLoad += AppDomainEventHandlers.OnAssemblyLoad;
            shadowCopyDomain.DomainUnload += AppDomainEventHandlers.OnDomainUnload;

            var originalPath = Path.Combine(shadowCopyDomain.BaseDirectory, PLUGIN_DIRECTORY_PATH, $"{ PLUGIN_ASSEMBLY_NAME}.exe");
            var backupPath = $@"{originalPath}.back";

            // assembly is locked?
            File.Delete(backupPath);
            File.Move(originalPath, backupPath);
            File.Copy(backupPath, originalPath);
            Console.WriteLine($"  <copy> \"{originalPath}\" => \"{backupPath}\"");

            var insrance1 = (IPlugable)shadowCopyDomain
                .CreateInstanceAndUnwrap(PLUGIN_ASSEMBLY_NAME, PLUGIN_EXECUTER_NAME);
            insrance1.DoAction();

            Console.WriteLine("--- Modify Begin.");
            PluginEditor.ModifyDoAction(20L, originalPath);
            Console.WriteLine("--- Modify End.");

            var insrance2 = (IPlugable)shadowCopyDomain
                .CreateInstanceAndUnwrap(PLUGIN_ASSEMBLY_NAME, PLUGIN_EXECUTER_NAME);
            insrance2.DoAction();

            AppDomain.Unload(shadowCopyDomain);

            var renewDomain = new AppDomainSetup()
                                .SetPrivateBinPath(PLUGIN_DIRECTORY_PATH)
                                .SetShadowCopy(PLUGIN_DIRECTORY_PATH, COPY_DIRECTORY_PATH)
                                .Build("Re-new Domain");
            Console.WriteLine(renewDomain.Dump());

            renewDomain.AssemblyLoad += AppDomainEventHandlers.OnAssemblyLoad;
            renewDomain.DomainUnload += AppDomainEventHandlers.OnDomainUnload;

            var insrance3 = (IPlugable)renewDomain
                .CreateInstanceAndUnwrap(PLUGIN_ASSEMBLY_NAME, PLUGIN_EXECUTER_NAME);
            insrance3.DoAction();

            AppDomain.Unload(renewDomain);

            File.Delete(originalPath);
            File.Move(backupPath, originalPath);
            Console.WriteLine($"  <restore> \"{backupPath}\" => \"{originalPath}\"");

            return;
        }

    }
}
