using System;
using System.Threading.Tasks;
using CommandLine;

namespace Examples.DLR.LateBinding
{
    [Verb("binding", HelpText = "Example of Late binding for DLR.")]
    internal class SubCommand
    {

        [Option('e', "enable", Default = Calling.All, Required = false,
            HelpText = "Enable method call [All|Dynamic|Company|Binder|Cache|Add].")]
        public Calling? EnabledCalling { get; set; }

        public async Task RunAsync()
        {
            Console.WriteLine("--- begin.");

            CallIfEnabled(Calling.Dynamic, LateBindingExample.DoBehindDynamicBinding);
            CallIfEnabled(Calling.Company, LateBindingExample.DoCompanyBindingFromFile);
            CallIfEnabled(Calling.Binder, LateBindingExample.DoUseObtrusiveBinder);
            CallIfEnabled(Calling.Cache, LateBindingExample.DoUseObtrusiveBinderForCheckCache);
            CallIfEnabled(Calling.Add, LateBindingExample.DoUseAddIntegerBinder);

            await Task.Delay(0).ConfigureAwait(false);
            Console.WriteLine("--- End.");
        }

        private void CallIfEnabled(Calling calling, Action action)
        {
            if ((Calling.All != this.EnabledCalling) && (calling != this.EnabledCalling))
            {
                return;
            }

            action();
        }

        public enum Calling
        {
            All = 0,
            Dynamic,
            Company,
            Binder,
            Cache,
            Add,
        }

    }

}
