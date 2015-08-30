// <copyright file="ParserContext.cs" company="Maletz, Josh" dateCreated="2015-08-26">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser
{
    using System;
    using System.IO;
    
    using MicroParser.Domain;

    /// <summary>
    /// The ParserContext takes care of the input and output files and executes the Parser to get the output.
    /// </summary>
    public class ParserContext
    {
        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }
        public string ShapedOutput { get; private set; }

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
        /// Parses the program and collects the output representation.
        /// </summary>
        public void ParseProgram()
        {
            var microParser = new MicroParser();
            microParser.Parse(this.inputProgram);

            this.ShapedOutput = microParser.Output;
        }

        /// <summary>
        /// Flushes the output to the specified filename. The default output 
        /// is shaped to see the nested calls as we parse.
        /// </summary>
        public void FlushOutput()
        {
            WriteDefaultOutputToFile();
        }

        private void WriteDefaultOutputToFile()
        {
            this.WriteFile(this.OutputFile, this.ShapedOutput);
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
