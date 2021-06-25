using System;
using System.Text;
using Mono.Cecil;

namespace Examples.Assemblies.AppDomains
{
    static class Extensions
    {
        public static string Dump(this AppDomain domain)
        {
            var message = new StringBuilder();
            message.AppendLine($"[{domain.FriendlyName}]");
            message.AppendLine($"\tApplication Base Path is: {domain.BaseDirectory}");
            message.AppendLine($"\tRelative search Path is: {domain.RelativeSearchPath}");
            message.AppendLine($"\tShadow copy cache Path is: {domain.SetupInformation.CachePath}");
            message.AppendLine($"\tShadow copy enabled is : {domain.ShadowCopyFiles}");
            return message.ToString();
        }

        public static string Dump(this MethodDefinition methos)
        {
            return string.Join(Environment.NewLine, methos.Body.Instructions);
        }

    }
}
