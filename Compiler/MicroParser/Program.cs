using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroParser
{
    class Program
    {
        static void Main(string[] args)
        {
            bool canContinue = GuardUsage(args);

            if (canContinue)
            {
                var parserContext = new ParserContext(args[0], args[1]);
                ExecuteParser(parserContext);
            }
        }

        private static void ExecuteParser(ParserContext parserContext)
        {
            parserContext.LoadInput();
            parserContext.ParseProgram();
            parserContext.FlushOutput();
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
            Console.WriteLine(@"MicroParser usage: $>microParser.exe path\to\input.txt path\to\output.txt");
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
