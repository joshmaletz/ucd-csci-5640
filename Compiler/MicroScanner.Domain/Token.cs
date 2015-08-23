// <copyright file="Token.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>
    
namespace MicroScanner.Domain
{
    /// <summary>
    /// Provides a base abstraction for tokens. Provides a name of the symbol and the value of the item represented 
    /// by the token.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public Token(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets the name of the token.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value of the token.
        /// </summary>
        /// <value>
        /// The value behind the token.
        /// </value>
        public string Value { get; private set; }
    }
}
