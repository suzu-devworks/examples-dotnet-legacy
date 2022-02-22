using System;
using System.Threading.Tasks;
using CommandLine;

namespace Examples.DLR.Expressions
{
    [Verb("expressions", HelpText = "Example of Expression Tree for DLR.")]
    internal class SubCommand
    {

        [Option('e', "enable", Default = Calling.All, Required = false,
            HelpText = "Enable method call [All|Hello|Calc|Array|Member].")]
        public Calling? EnabledCalling { get; set; }

        public async Task RunAsync()
        {
            Console.WriteLine("--- begin.");

            CallIfEnabled(Calling.Hello, ExpressionExample.SayHello);
            CallIfEnabled(Calling.Calc, ExpressionExample.Calculate);
            CallIfEnabled(Calling.Array, ExpressionExample.DoArrayAccess);
            CallIfEnabled(Calling.Member, ExpressionExample.DoMemberAccess);

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
            Hello,
            Calc,
            Array,
            Member,
        }

    }

}
