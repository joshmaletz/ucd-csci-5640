using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            bool canContinue = GuardUsage(args);

            if (canContinue)
            {
                var scannerContext = new ScannerContext(args[0], args[1]);
                ExecuteScanner(scannerContext);
            }
        }

        private static void ExecuteScanner(ScannerContext scannerContext)
        {
            scannerContext.LoadInput();
            scannerContext.ScanProgram();
            scannerContext.FlushOutput();

            PrintOutputToScreen(scannerContext);
        }

        private static void PrintOutputToScreen(ScannerContext scannerContext)
        {
            Console.WriteLine();
            Console.WriteLine("=============================================");
            Console.WriteLine("Tokens for input program: ");
            var tokens = scannerContext.Tokens;
            
            foreach (var token in tokens)
            {
                Console.WriteLine("\t{0} - {1}", token.Name, token.Value);
            }
            
            Console.WriteLine();
            Console.WriteLine("=============================================");
            Console.WriteLine();
        }

        private static bool GuardUsage(string[] args)
        {
            bool canContinue = args.Length == 2;

            if (!canContinue)
            {
                WriteUsage();
            }

            return canContinue;
        }

        private static void WriteUsage()
        {
            Console.WriteLine("");
            Console.WriteLine(@"MicroScanner usage: $>microscanner.exe path\to\input.txt path\to\output.txt");
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
