// <copyright file="SyntaxErrorException.cs" company="Maletz, Josh" dateCreated="2015-08-26">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    using System;

    /// <summary>
    /// This is an exception for wrapping syntax errors.
    /// </summary>
    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(string message)
            : base(message)
        {
        }
    }
}
