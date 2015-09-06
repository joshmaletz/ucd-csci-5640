// <copyright file="MicroGenerator.cs" company="Maletz, Josh" dateCreated="2015-09-06">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    using System.Text;

    /// <summary>
    /// The MicroGenerator is just a glorified string buffer. Rather than write to a file on demand, 
    /// we build up and store the machine code to generate so that we can redirect the output from 
    /// the parser context. 
    /// </summary>
    public class MicroGenerator
    {
        private readonly StringBuilder outputBuilder = new StringBuilder();

        /// <summary>
        /// Write a single command to the code file.
        /// </summary>
        /// <param target="command"></param>
        public void Generate(string command)
        {
            this.outputBuilder.AppendLine(command);
        }

        /// <summary>
        /// Write the machine code.
        /// </summary>
        /// <param target="command"></param>
        /// <param target="value"></param>
        /// <param target="type"></param>
        public void Generate(string command, string value, string type)
        {
            string line = string.Format("{0} {1}, {2}", command, value, type);
            this.outputBuilder.AppendLine(line);
        }

        /// <summary>
        /// Wrte the machine code.
        /// </summary>
        /// <param target="command"></param>
        /// <param target="source1"></param>
        /// <param target="source2"></param>
        /// <param target="target"></param>
        public void Generate(string command, string source1, string source2, string target)
        {
            string line = string.Format("{0} {1}, {2}, {3}", command, source1, source2, target);
            this.outputBuilder.AppendLine(line);
        }

        /// <summary>
        /// Return the machine code buffer contents as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.outputBuilder.ToString();
        }
    }
}
