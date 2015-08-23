namespace MicroScanner
{
    using System;

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
