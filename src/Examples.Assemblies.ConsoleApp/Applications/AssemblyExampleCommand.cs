using System;
using System.Collections.Generic;
using System.Reflection;
using Examples.Assemblies.AppDomains;
using Examples.Assemblies.Extensions;
using Mono.Options;

namespace Examples.Assemblies.Applications
{
    internal class AssemblyExampleCommand : Command
    {
        public AssemblyExampleCommand()
            : base("assembly", "Assembly examples.")
        {
            var exename = Assembly.GetEntryAssembly().GetFileName();

            this.Options = new OptionSet()
            {
                $"usage: {exename} assembly  [--option] [<args>]",
                //"",
                //"Options:",
                //{ "n|name=", "the {NAME} of someone to greet.", v => { } },
                //{ "v", "increase debug message verbosity", v => { Console.WriteLine(v);  } },
            };
        }

        public override int Invoke(IEnumerable<string> arguments)
        {
            Console.WriteLine($"■ {nameof(AppDomainExample.DoCreateAppDomains)}");
            AppDomainExample.DoCreateAppDomains();
            Console.WriteLine();

            Console.WriteLine($"■ {nameof(AppDomainExample.DoUnloadAppDomains)}");
            AppDomainExample.DoUnloadAppDomains();
            Console.WriteLine();

            Console.WriteLine($"■ {nameof(AppDomainExample.DoExecuteAssembly)}");
            AppDomainExample.DoExecuteAssembly();
            Console.WriteLine();

            Console.WriteLine($"■ {nameof(AppDomainExample.DoDoCallBack)}");
            AppDomainExample.DoDoCallBack();
            Console.WriteLine();

            Console.WriteLine($"■ {nameof(AppDomainExample.DoCreateInstance)}");
            AppDomainExample.DoCreateInstance();
            Console.WriteLine();

            Console.WriteLine($"■ {nameof(AppDomainExample.DoHandleException)}");
            AppDomainExample.DoHandleException();
            Console.WriteLine();

            Console.WriteLine($"■ {nameof(ShadowCopyExample.DoCreateShadowCopyInstance)}");
            ShadowCopyExample.DoCreateShadowCopyInstance();
            Console.WriteLine();

            Console.WriteLine($"■ {nameof(ShadowCopyExample.DoReloadDomain)}");
            ShadowCopyExample.DoReloadDomain();
            Console.WriteLine();

            return base.Invoke(arguments);
        }

    }
}
