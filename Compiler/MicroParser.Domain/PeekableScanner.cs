// <copyright file="PeekableScanner.cs" company="Maletz, Josh" dateCreated="2015-08-26">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    using System.Collections.Generic;
    using MicroScanner.Domain;

    /// <summary>
    /// The PeekableScanner is a wrapper around my MicroScanner to allow us to lookahead into the token list.
    /// It is a very simple implementation which just calls ScanAll  on creation and adds all the tokens to 
    /// a strongly-typed queue to allow for peeking. Once we have the plans for the creation of hte MicroGenerator,
    /// I might build the functionality intothe MicroScanner for NextToken, but will wait and see.
    /// </summary>
    public class PeekableScanner
    {
        private readonly Queue<Token> tokenQueue;

        /// <summary>
        /// Wrapper is simple:
        /// - Create a queue for tokens
        /// - Scan all tokens
        /// - add each token in order to the queue
        /// </summary>
        /// <param name="scanner"></param>
        public PeekableScanner(MicroScanner scanner)
        {
            tokenQueue = new Queue<Token>();
            scanner.ScanAll();
            var tokensInOrder = new List<Token>(scanner.Output);

            foreach (Token token in tokensInOrder)
            {
                tokenQueue.Enqueue(token);
            }
        }

        /// <summary>
        /// A call to scan removes the token fro mthe structure.
        /// </summary>
        /// <returns></returns>
        public Token Scan()
        {
            return tokenQueue.Dequeue();
        }

        /// <summary>
        /// A call to NextToken just peeks at the next item in the queue without removing it.
        /// </summary>
        /// <returns></returns>
        public Token NextToken()
        {
            return tokenQueue.Peek();
        }
    }
}