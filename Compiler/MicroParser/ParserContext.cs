// <copyright file="ParserContext.cs" company="Maletz, Josh" dateCreated="2015-08-26">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

using System.Text;

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
        public string MachineCode { get; private set; }
        private CompilerOutput compilerOutput = null;
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
                Console.WriteLine("Could not read input file. Details: {0}", e.Message);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Parses the program and collects the output representation.
        /// </summary>
        public void ParseProgram()
        {
            var microParser = new MicroParser();

            try
            {
                microParser.Parse(this.inputProgram);
            }
            catch (SyntaxErrorException see)
            {
                Console.WriteLine("Cannot parse program - syntax error detected.");    
                Console.WriteLine("Details: {0}", see.Message);    
                Console.WriteLine();    
                Console.WriteLine("Flushing output.");    
            }

            this.ShapedOutput = microParser.Output;
            this.MachineCode = microParser.MachineCode;
            this.compilerOutput = microParser.CompilerParseActionList;
        }

        /// <summary>
        /// Flushes the output to the specified filename. The default output 
        /// is shaped to see the nested calls as we parse.
        /// </summary>
        public void FlushOutput()
        {
            WriteDefaultOutputToFile();
            WriteMachineCodeToFile();
            WriteParseActionsToFile();
        }

        private void WriteDefaultOutputToFile()
        {
            this.WriteFile(this.OutputFile, this.ShapedOutput);
        }

        private void WriteMachineCodeToFile()
        {
            this.WriteFile(this.GetMachineCodeOutputPath(), this.MachineCode);
        }

        private void WriteParseActionsToFile()
        {
            this.WriteFile(this.GetParseActionOutputPath(), GenerateParseActionList());
        }

        /// <summary>
        /// Function for creating the content of our 'parseActions' output file. 
        /// </summary>
        /// <returns></returns>
        private string GenerateParseActionList()
        {
            var actionBuilder = new StringBuilder();

            foreach (var record in this.compilerOutput.ParseActionRecords)
            {
                actionBuilder.AppendLine(string.Format("Action: {0}", record.ParseAction));
                actionBuilder.AppendLine(string.Format("Remaining Tokens: {0}", record.RemainingTokens));
                actionBuilder.AppendLine("Generated Code:");
                actionBuilder.AppendLine(record.GeneratedCodeThusFar);
                actionBuilder.AppendLine(
                    "============================================================================================");
            }

            return actionBuilder.ToString();
        }

        private string GetParseActionOutputPath()
        {
            return Path.ChangeExtension(
                this.OutputFile,
                string.Format("parseActions{0}", Path.GetExtension(this.OutputFile)));
        }

        private string GetMachineCodeOutputPath()
        {
            return Path.ChangeExtension(
                this.OutputFile,
                string.Format("machineCode{0}", Path.GetExtension(this.OutputFile)));
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
