using System;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Examples.WindowsService.Helpers;

namespace Examples.WindowsService
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; ;
            if (Environment.UserInteractive)
            {
                DoProcessInstall(args);
            }
            else
            {
                var service = new Service1();
                ServiceBase.Run(service);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "error.txt");
            File.AppendAllText(path, ((Exception)e.ExceptionObject).Message
                                    + ((Exception)e.ExceptionObject).InnerException.Message);
        }

        private static void DoProcessInstall(string[] args)
        {
            // Use Console, *.csproj <OutputType> WinExe to Exe.

            switch (args)
            {
                case var x when x.Contains("--install"):
                    //This API supports the product infrastructure and
                    // is not intended to be used directly from your code.
                    ManagedInstallerClass.InstallHelper(new string[] {
                        Assembly.GetExecutingAssembly().Location });
                    break;

                case var x when x.Contains("--uninstall"):
                    ManagedInstallerClass.InstallHelper(new string[] {
                        "/u", Assembly.GetExecutingAssembly().Location });
                    break;

                case var x when x.Contains("--installapi"):
                    ServiceInstallHelper.InstallAndStart("ServiceName", "ServiceDisplayName",
                        Assembly.GetExecutingAssembly().Location);
                    Console.WriteLine("Service Installed.");
                    break;

                case var x when x.Contains("--uninstallapi"):
                    ServiceInstallHelper.Uninstall("ServiceName");
                    Console.WriteLine("Service Uninstalled.");
                    break;

                default:
                    var name = Assembly.GetExecutingAssembly().GetName().Name;
                    Console.Error.WriteLine($"Usage: {name} --install|--uninstall|--installapi|--uninstallapi");
                    break;
            }

            return;
        }

    }
}
