// <copyright file="ParseActionRecord.cs" company="Maletz, Josh" dateCreated="2015-09-06">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    /// <summary>
    /// A record serving as a parse log so we can track what is happening, and what both the input and output 
    /// look like at the time the action is called.
    /// </summary>
    public class ParseActionRecord
    {
        public string ParseAction { get; private set; }
        public string RemainingTokens { get; private set; }
        public string GeneratedCodeThusFar { get; private set; }

        private ParseActionRecord()
        {
            
        }

        /// <summary>
        /// Factory method for creating a new record to store.
        /// </summary>
        /// <param name="action">Action to 'log' from parser</param>
        /// <param name="tokenList">All remaining tokens</param>
        /// <param name="generatedOutput">All output up to point this action is called.</param>
        /// <returns></returns>
        public static ParseActionRecord Create(string action, string tokenList, string generatedOutput)
        {
            return new ParseActionRecord()
            {
                GeneratedCodeThusFar = generatedOutput,
                ParseAction = action,
                RemainingTokens = tokenList
            };
        }
    }
}
