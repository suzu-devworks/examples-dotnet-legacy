using System;
using System.Threading.Tasks;
using CommandLine;

namespace Examples.DLR.ExcelCom
{
    [Verb("excel", HelpText = "Example of Excel COM for DLR.")]
    internal class SubCommand
    {

        [Option('f', "file", Required = false, Default = @"SampleBook.xlsx",
            HelpText = "Python scripting file path.")]
        public string ExcelFilePath { get; set; }

        public async Task RunAsync()
        {
            Console.WriteLine("--- begin.");

            ExcelComExample.DoCreateExcel(this.ExcelFilePath);

            await Task.Delay(0).ConfigureAwait(false);
            Console.WriteLine("--- End.");
        }

    }

}
