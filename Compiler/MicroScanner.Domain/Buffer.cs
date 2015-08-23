// <copyright file="Buffer.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroScanner.Domain
{
    using System.Text;

    /// <summary>
    /// This Buffer object is used to store characters as we build tokens. Once a token is identified and created, we 
    /// flush the buffer.
    /// </summary>
    public class Buffer
    {
        /// <summary>
        /// The internal structure for the buffer - the humble <see cref="StringBuilder"/>.
        /// </summary>
        private StringBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="Buffer"/> class.
        /// </summary>
        public Buffer()
        {
            this.builder = new StringBuilder();
        }

        /// <summary>
        /// Adds the specified value to the buffer.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Add(char value)
        {
            this.builder.Append(value);
        }

        /// <summary>
        /// Clears the buffer contents.
        /// </summary>
        public void Clear()
        {
            this.builder.Clear();
        }

        /// <summary>
        /// Flushes this instance - by flush we mean we return the contents of the buffer and then clear it.
        /// </summary>
        /// <returns></returns>
        public string Flush()
        {
            string currentValue = this.builder.ToString();
            this.Clear();
            return currentValue;
        }
    }
}
