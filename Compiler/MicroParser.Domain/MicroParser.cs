// <copyright file="MicroParser.cs" company="Maletz, Josh" dateCreated="2015-08-26">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    using System;
    using MicroScanner.Domain;

    /// <summary>
    /// The MicroParser is the main focus of this assignment. It follows the 
    /// algorithm as defiend in our classwork. There are a couple of possbily 
    /// important tweaks to note. My Scanner, as implemented last week, does 
    /// not support a "NextToken" look ahead. As I am not yet sure what will 
    /// be needed to build the MicroGeneraator, I held off on refactoring the
    /// existing scanner, and instead wrapped it in a 
    ///"Peekable Scanner" abstraction. All this does is create the illusion that 
    /// the scanner itself is allowing us to peek ahead. See the <see cref="PeekableScanner"/> 
    /// for more details.
    /// </summary>
    public class MicroParser
    {
        private PeekableScanner peekScanner = null;
        private ShapedWriter shapedWriter = new ShapedWriter();

        /// <summary>
        /// This returns the Parser output as a nested, shaped string.
        /// </summary>
        public string Output
        {
            get { return this.shapedWriter.Content; }
        }

        /// <summary>
        /// We create a microscanner, wrap it in a peekable scanner, and then start the
        /// parsing routine by calling SystemGoal. In order to support the shaped output,
        /// you will see all method calls preceded and followed by shapewriter push/pop calls.
        /// For simplicity's sake, I left it like this, but would most likely wrap this in 
        /// a nested lambda call to the shapewriter to call the actual methods. Wasn't sure 
        /// where this is evolving with the micro generator, so might do that next week depending. 
        /// I don't like having the output format tied to the algorithm. Hence trying to create 
        /// the parse tree during and then formatting on that structure after the fact.
        /// </summary>
        /// <param name="program"></param>
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
}
