using System;
using System.Threading.Tasks;
using CommandLine;

namespace Examples.DLR.Dynamic
{
    [Verb("dynamic", HelpText = "Example of Dynamic object for DLR.")]
    internal class SubCommand
    {

        [Option('e', "enable", Default = Calling.All, Required = false,
            HelpText = "Enable method call [All|Book|Book2|UseEx|ModEx|RemEx|AddEx].")]
        public Calling? EnabledCalling { get; set; }

        public async Task RunAsync()
        {
            Console.WriteLine("--- begin.");

            CallIfEnabled(Calling.Book, DynamicExample.DoDynamicBook);
            CallIfEnabled(Calling.Book2, DynamicExample.DoDynamicBook2);
            CallIfEnabled(Calling.UseEx, DynamicExample.DoUseExpandoObject);
            CallIfEnabled(Calling.ModEx, DynamicExample.DoModifyProperty);
            CallIfEnabled(Calling.RemEx, DynamicExample.DoRemoveProperty);
            CallIfEnabled(Calling.AddEx, DynamicExample.DoAddMethod);

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
            Book,
            Book2,
            UseEx,
            ModEx,
            RemEx,
            AddEx,
        }

    }

}
