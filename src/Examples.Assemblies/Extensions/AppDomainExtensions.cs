using System;
using System.IO;
using System.Text;

namespace Examples.Assemblies.Extensions
{
    public static class AppDomainExtensions
    {

        public static AppDomain Build(this AppDomainSetup setup, string friendlyName)
        {
            var domain = AppDomain.CreateDomain(friendlyName, null, setup);

            return domain;
        }


        public static AppDomainSetup SetShadowCopy(this AppDomainSetup setup, string localAssemblyPath, string cachePath, string applicationName = "Shadow Copy Domain")
        {
            setup.ShadowCopyFiles = true.ToString();    //required.
            setup.ShadowCopyDirectories =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, localAssemblyPath);     //option.
            setup.CachePath = cachePath;    //required.
            setup.ApplicationName = applicationName;   //option.

            return setup;
        }


        public static AppDomainSetup SetPrivateBinPath(this AppDomainSetup setup, string path)
        {
            setup.PrivateBinPath = path;

            return setup;

        }


        public static AppDomainSetup SetConfigurationFile(this AppDomainSetup setup, string path)
        {
            setup.ConfigurationFile = path;

            return setup;
        }


        public static string Dump(this AppDomain domain)
        {
            const string indent = "  ";

            var message = new StringBuilder();
            message.AppendLine($"[{domain.FriendlyName}]");
            message.AppendLine($"{indent}Application Base Path is: {domain.BaseDirectory}");
            message.AppendLine($"{indent}Relative search Path is: {domain.RelativeSearchPath}");
            message.AppendLine($"{indent}ConfigurationFile is: {domain.SetupInformation.ConfigurationFile}");
            message.AppendLine($"{indent}Shadow copy cache Path is: {domain.SetupInformation.CachePath}");
            message.AppendLine($"{indent}Shadow copy enabled is : {domain.ShadowCopyFiles}");

            return message.ToString();
        }


    }
}
