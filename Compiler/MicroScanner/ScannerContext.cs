namespace MicroScanner
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    
    using MicroScanner.Domain;

    public class ScannerContext
    {
        public string InputFile { get; private set; }
        public string OutputFile { get; private set; }
        public List<Token> Tokens = new List<Token>();

        private string inputProgram = string.Empty;

        public ScannerContext(string inputFile, string outputFile)
        {
            this.InputFile = inputFile;
            this.OutputFile = outputFile;
        }

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

        public void ScanProgram()
        {
           var microScanner = new MicroScanner(this.inputProgram);
            microScanner.ScanAll();

            this.Tokens = new List<Token>(microScanner.Output);
        }

        public void FlushOutput()
        {
            WriteTokensToFile();
            WriteTokensWithMatchesToFile();
        }

        private void WriteTokensToFile()
        {
            var tokenOutput = new StringBuilder(); 
            foreach (var token in this.Tokens)
            {
                tokenOutput.AppendLine(token.Name);
            }

            this.WriteFile(this.OutputFile, tokenOutput.ToString());
        }

        private void WriteTokensWithMatchesToFile()
        {
            var tokenOutput = new StringBuilder();
            tokenOutput.AppendLine("Token\t\tMatched-Value");
            tokenOutput.AppendLine("-----\t\t-------------");

            foreach (var token in this.Tokens)
            {
                tokenOutput.AppendLine(
                    string.Format("{0}{1}{2}", token.Name, token.Name.Length >= 8 ? "\t" : "\t\t", token.Value));
            }

            this.WriteFile(this.GetVerboseOutputPath(), tokenOutput.ToString());
        }

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
