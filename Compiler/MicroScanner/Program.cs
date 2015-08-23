// <copyright file="Program.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroScanner
{
    using System;

    /// <summary>
    /// <para>This is the main entry point of the command-line runner for the Micro Scanner. It creates a ScannerContext
    /// object by passing in the input file path and the desired output file path. The context is then told to execute 
    /// the algorithms.
    /// </para>
    /// <para>
    /// The entry point also checks to make sure paths are supplied, though for this version we don't do any validation 
    /// on the paths.
    /// </para>
    /// </summary>
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
