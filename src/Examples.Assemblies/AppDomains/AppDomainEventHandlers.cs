using System;
using System.Runtime.ExceptionServices;

namespace Examples.Assemblies.AppDomains
{
    public static class AppDomainEventHandlers
    {

        public static void OnDomainUnload(object sender, EventArgs e)
        {
            var domain = AppDomain.CurrentDomain;
            Console.WriteLine($"--> OnDomainUnload[{domain.FriendlyName}].");
        }

        public static void OnFirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            var domain = AppDomain.CurrentDomain;
            Console.Error.WriteLine($"--> OnFirstChanceException[{domain.FriendlyName}]: {e.Exception.Message}");
        }

        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var domain = AppDomain.CurrentDomain;
            Console.Error.WriteLine($"--> OnDomainUnload[{domain.FriendlyName}]: {e.ExceptionObject}");
        }

        public static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            var domain = AppDomain.CurrentDomain;
            Console.WriteLine($"--> OnAssemblyLoad[{domain.FriendlyName}]: {args.LoadedAssembly.FullName}");
        }

    }
}
