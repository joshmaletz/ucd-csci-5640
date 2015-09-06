// <copyright file="CompilerOutput.cs" company="Maletz, Josh" dateCreated="2015-09-06">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Simple container to store parse action records as requests for the assignment output.
    /// </summary>
    public class CompilerOutput
    {
        public List<ParseActionRecord> ParseActionRecords;

        public CompilerOutput()
        {
            this.ParseActionRecords = new List<ParseActionRecord>();
        }

        public void AddRecord(ParseActionRecord record)
        {
            this.ParseActionRecords.Add(record);
        }
    }
}
