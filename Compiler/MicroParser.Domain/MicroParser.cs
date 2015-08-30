using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroParser.Domain
{
    using MicroScanner.Domain;

    public class MicroParser
    {
        private PeekableScanner peekScanner = null;
        private ShapedWriter shapedWriter = new ShapedWriter();

        public string Output
        {
            get { return this.shapedWriter.Content; }
        }

        public void Parse(string program)
        {
            this.microScanner = new MicroScanner(program);
            this.peekScanner = new PeekableScanner(this.microScanner);

            SystemGoal();
        }

        private void SystemGoal()
        {
            this.shapedWriter.PushWrite("<SystemGoal>");

            Program();
            Match("EofSym");

            this.shapedWriter.PopTab();
        }

        private void Program()
        {
            this.shapedWriter.PushWrite("<program>");

            Match("BeginSym");
            StatementList();
            Match("EndSym");

            this.shapedWriter.PopTab();
        }

        private void StatementList()
        {
            this.shapedWriter.PushWrite("<statement list>");

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

            this.shapedWriter.PopTab();
        }

        private void Statement()
        {
            this.shapedWriter.PushWrite("<statement>");

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

            this.shapedWriter.PopTab();
        }

        private void IdList()
        {
            this.shapedWriter.PushWrite("<id list>");

            Ident();
            if ("Comma".Equals(NextToken().Name, StringComparison.OrdinalIgnoreCase))
            {
                Match("Comma");
                IdList();
            }

            this.shapedWriter.PopTab();
        }

        private void ExprList()
        {
            this.shapedWriter.PushWrite("<expr list>");

            Expression();
            if ("Comma".Equals(NextToken().Name, StringComparison.OrdinalIgnoreCase))
            {
                Match("Comma");
                ExprList();
            }

            this.shapedWriter.PopTab();
        }

        private void Expression()
        {
            this.shapedWriter.PushWrite("<expression>");

            Primary();
            var nextToken = NextToken();
            if ("PlusOp".Equals(nextToken.Name, StringComparison.OrdinalIgnoreCase) || "MinusOp".Equals(nextToken.Name, StringComparison.OrdinalIgnoreCase))
            {
                AddOp();
                Expression();
            }

            this.shapedWriter.PopTab();
        }

        private void Primary()
        {
            this.shapedWriter.PushWrite("<primary>");

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

            this.shapedWriter.PopTab();
        }

        private void Ident()
        {
            //Console.WriteLine("<ident>");

            Match("Id");
        }

        private void AddOp()
        {
            this.shapedWriter.PushWrite("<add op>");

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

            this.shapedWriter.PopTab();
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

            this.shapedWriter.PushWrite(currentToken.Name);
            this.shapedWriter.PopTab();
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
