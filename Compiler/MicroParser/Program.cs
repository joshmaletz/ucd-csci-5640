﻿// <copyright file="Program.cs" company="Maletz, Josh" dateCreated="2015-08-26">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser
{
    using System;

    class Program
    {
        /// <summary>
        /// <para>This is the main entry point of the command-line runner for the Micro Parser. It creates a ParserContext
        /// object by passing in the input file path and the desired output file path. The context is then told to execute 
        /// the algorithms.
        /// </para>
        /// <para>
        /// The entry point also checks to make sure paths are supplied, though for this version we don't do any validation 
        /// on the paths.
        /// </para>
        /// </summary>
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
