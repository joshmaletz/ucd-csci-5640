using System.Collections.Generic;
using MicroScanner.Domain;

namespace MicroParser.Domain
{
    public class PeekableScanner
    {
        private readonly Queue<Token> tokenQueue;

        public PeekableScanner(MicroScanner.Domain.MicroScanner scanner)
        {
            tokenQueue = new Queue<Token>();
            scanner.ScanAll();
            var tokensInOrder = new List<Token>(scanner.Output);

            foreach (Token token in tokensInOrder)
            {
                tokenQueue.Enqueue(token);
            }
        }

        public Token Scan()
        {
            return tokenQueue.Dequeue();
        }

        public Token NextToken()
        {
            return tokenQueue.Peek();
        }
    }
}