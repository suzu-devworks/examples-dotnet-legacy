using System;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Scripting.Hosting;

namespace Examples.DLR.Scripting
{
    [Verb("scripting", HelpText = "Example of Python scripting file load and execute in C#.")]
    internal class SubCommand
    {
        [Option('f', "file", Required = false, Default = @"Scripting/hello_world.py",
            HelpText = "Python scripting file path.")]
        public string ScriptingFilePath { get; set; }

        public async Task RunAsync()
        {
            Console.WriteLine("--- begin.");

            ScriptRuntime runtime = IronPython.Hosting.Python.CreateRuntime();
            dynamic scope = runtime.UseFile(@"Scripting/HelloWorld.py");
            scope.Hello();

            Parson parson = new Parson() { FirstName = "Foo", LastName="Bar" };
            scope.Hi(parson);

            await Task.Delay(0).ConfigureAwait(false);
            Console.WriteLine("--- End.");
        }

    }

}
