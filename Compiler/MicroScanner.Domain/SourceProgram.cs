// <copyright file="SourceProgram.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroScanner.Domain
{
    using System;

    /// <summary>
    /// This is an abstraction around the input program that lets us treat it more like a queue of characters. This 
    /// makes the logic a bit easier to see and we don't have to have all of the actual character/file io code to
    /// block what we are trying to do with the scanner.
    /// </summary>
    public class SourceProgram
    {
        /// <summary>
        /// The source program
        /// </summary>
        private string sourceProgram = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceProgram"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public SourceProgram(string input)
        {
            this.sourceProgram = input;
        }

        /// <summary>
        /// Advances a single character position in the program. Does not return anything.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">No more source to advance.</exception>
        public void Advance()
        {
            if (this.sourceProgram.Length <= 0)
            {
                throw new InvalidOperationException("No more source to advance.");
            }
           
            this.sourceProgram = this.sourceProgram.Substring(1, this.sourceProgram.Length - 1);
        }

        /// <summary>
        /// Inspects the next character, but does not remove it from the source stream.
        /// </summary>
        /// <returns>The character at the current pointer.</returns>
        public char Inspect()
        {
            char nextChar = char.MinValue;
            if (this.sourceProgram.Length > 0)
            {
                nextChar = this.sourceProgram[0];
            }

            return nextChar;
        }

        /// <summary>
        /// Reads the current character. This returns the character at the current pointer and then advances the 
        /// pointer one position.
        /// </summary>
        /// <returns>the character at the current pointer.</returns>
        public char ReadCurrentCharacter()
        {
            char nextChar = char.MinValue;
            if (this.sourceProgram.Length > 0)
            {
                nextChar = this.sourceProgram[0];
                this.sourceProgram = this.sourceProgram.Substring(1, this.sourceProgram.Length - 1);
            }            

            return nextChar;
        }

        /// <summary>
        /// Determines whether the character pointer is at the end of the file.
        /// </summary>
        /// <returns><c>true</c> if at the end of the file; otherwise <c>false</c>.</returns>
        public bool IsAtEnd()
        {
            return 0.Equals(this.sourceProgram.Length);
        }
    }
}
