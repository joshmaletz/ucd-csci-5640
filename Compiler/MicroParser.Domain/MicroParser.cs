using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroParser.Domain
{
    using MicroScanner.Domain;

    public class PeekableScanner {

        public PeekableScanner (MicroScanner scanner){

            tokenQueue = new Queue<Token>();
            scanner.ScanAll();
            List<Token> tokensInOrder = new List<Token>(scanner.Output);

            foreach (var token in tokensInOrder)
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

        private Queue<Token> tokenQueue = null;

    } 

    public class MicroParser
    {
        private PeekableScanner peekScanner = null;

        public void Parse(string program)
        {
            this.microScanner = new MicroScanner(program);
            this.peekScanner = new PeekableScanner(this.microScanner);

            SystemGoal();
        }

        private void SystemGoal()
        {
            Console.WriteLine("<SystemGoal>");

            Program();
            Match("EofSym");

            
        }

        private void Program()
        {
            Console.WriteLine("<program>");

            Match("BeginSym");
            StatementList();
            Match("EndSym");
        }

        private void StatementList()
        {
            Console.WriteLine("<statement list>");

            Statement();
            var nextToken = NextToken();
            switch (nextToken.Name)
            {
                case "Id":
                case "ReadSym":
                case "WriteSym":
                    StatementList();
                    break;
                default:
                    break;
            }
        }

        private void Statement()
        {
            Console.WriteLine("<statement>");

            var nextToken = NextToken();
            switch (nextToken.Name)
            {
                case "Id":
                    Ident();
                    Match("AssignOp");
                    Expression();
                    Match("SemiColon");
                    break;
                case "ReadSym":
                    Match("ReadSym");
                    Match("LParen");
                    IdList();
                    Match("RParen");
                    Match("SemiColon");
                    break;
                case "WriteSym":
                    Match("WriteSym");
                    Match("LParen");
                    ExprList();
                    Match("RParen");
                    Match("SemiColon");
                    break;
                default:
                    SyntaxError(nextToken);
                    break;
            }
        }

        private void IdList()
        {
            Console.WriteLine("<id list>");

            Ident();
            if ("Comma".Equals(NextToken().Name, StringComparison.OrdinalIgnoreCase))
            {
                Match("Comma");
                IdList();
            }
        }

        private void ExprList()
        {
            Console.WriteLine("<expr list>");

            Expression();
            if ("Comma".Equals(NextToken().Name, StringComparison.OrdinalIgnoreCase))
            {
                Match("Comma");
                ExprList();
            }
        }

        private void Expression()
        {
            Console.WriteLine("<expression>");

            Primary();
            var nextToken = NextToken();
            if ("PlusOp".Equals(nextToken.Name, StringComparison.OrdinalIgnoreCase) || "MinusOp".Equals(nextToken.Name, StringComparison.OrdinalIgnoreCase))
            {
                AddOp();
                Expression();
            }
        }

        private void Primary()
        {
            Console.WriteLine("<primary>");

            var nextToken = NextToken();
            switch (nextToken.Name)
            {
                case "LParen":
                    Match("LParen");
                    Expression();
                    Match("RParen");
                    break;
                case "Id":
                    Ident();
                    break;
                case "IntLiteral":
                    Match("IntLiteral");
                    break;
                default:
                    SyntaxError(nextToken);
                    break;
            }
        }

        private void Ident()
        {
            //Console.WriteLine("<ident>");

            Match("Id");
        }

        private void AddOp()
        {
            Console.WriteLine("<add op>");

            var nextToken = NextToken();
            switch (nextToken.Name)
            {
                case "PlusOp":
                    Match("PlusOp");
                    break;
                case "MinusOp":
                    Match("MinusOp");
                    break;
                default:
                    SyntaxError(nextToken);
                    break;
            }
        }

        private Token NextToken()
        {
            return this.peekScanner.NextToken();
            //return new Token("Null", "Null-Must implement");
        }

        private void Match(string legalTokenName)
        {
            var currentToken = this.Scan();
            if (!legalTokenName.Equals(currentToken.Name, StringComparison.OrdinalIgnoreCase))
            {
                SyntaxError(currentToken);
            }

            Console.WriteLine(currentToken.Name);
        }

        private Token Scan()
        {
           // this.peekScanner.Scan();
           // return this.microScanner.Scan();
            return this.peekScanner.Scan();
        }

        private void SyntaxError(Token unexpectedToken)
        {
            throw new SyntaxErrorException(
                string.Format(
                "Unexpected token '{0}' with value '{1}'", 
                unexpectedToken.Name, 
                unexpectedToken.Value));
        }

        private MicroScanner microScanner = null;
    }

    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(string message) : base(message)
        {
        }
    }
}
