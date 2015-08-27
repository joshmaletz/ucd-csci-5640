// <copyright file="ScannerContext.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    
    using MicroParser.Domain;

    /// <summary>
    /// The ScannerContext takes care of the input and output files and executes the Scanner to get the output.
    /// </summary>
    public class ParserContext
    {
        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }
        //public List<Token> Tokens = new List<Token>();

        private string inputProgram = string.Empty;

        public ParserContext(string inputFile, string outputFile)
        {
            this.InputFile = inputFile;
            this.OutputFile = outputFile;
        }

        /// <summary>
        /// Loads the input file into the SourceProgram abstraction.
        /// </summary>
        public void LoadInput()
        {
            try
            {
                this.inputProgram = File.ReadAllText(this.InputFile);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read input file. Details: {0}", e.ToString());
            }
        }

        /// <summary>
        /// Scans the program and collects the output tokens.
        /// </summary>
        public void ParseProgram()
        {
            var microParser = new MicroParser();
            microParser.Parse(this.inputProgram);

            //this.Tokens = new List<Token>(microScanner.Output);
        }

        /// <summary>
        /// Flushes the output to the specified filename, and a verbose file with matches for testing purposes.
        /// </summary>
        public void FlushOutput()
        {
            //WriteTokensToFile();
            //WriteTokensWithMatchesToFile();
        }

        //private void WriteTokensToFile()
        //{
        //    var tokenOutput = new StringBuilder(); 
        //    foreach (var token in this.Tokens)
        //    {
        //        tokenOutput.AppendLine(token.Name);
        //    }

        //    this.WriteFile(this.OutputFile, tokenOutput.ToString());
        //}

        //private void WriteTokensWithMatchesToFile()
        //{
        //    var tokenOutput = new StringBuilder();
        //    tokenOutput.AppendLine("Token\t\tMatched-Value");
        //    tokenOutput.AppendLine("-----\t\t-------------");

        //    foreach (var token in this.Tokens)
        //    {
        //        tokenOutput.AppendLine(
        //            string.Format("{0}{1}{2}", token.Name, token.Name.Length >= 8 ? "\t" : "\t\t", token.Value));
        //    }

        //    this.WriteFile(this.GetVerboseOutputPath(), tokenOutput.ToString());
        //}

        private string GetVerboseOutputPath()
        {
            return Path.ChangeExtension(
                this.OutputFile,
                string.Format("verbose{0}", Path.GetExtension(this.OutputFile)));
        }

        private void WriteFile(string path, string contents)
        {
            Console.WriteLine("Creating output file: {0}", path);

            try
            {
                File.WriteAllText(path, contents);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not write output file. Details: {0}", e.ToString());
            }
        }
    }
}
