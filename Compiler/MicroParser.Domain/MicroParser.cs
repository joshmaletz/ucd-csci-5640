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
        private MicroScanner microScanner = null;
        private SemanticRoutines semanticRoutines;
        private CompilerOutput compilerOutput = new CompilerOutput();
        
        /// <summary>
        /// This returns the Parser output as a nested, shaped string.
        /// </summary>
        public string Output
        {
            get { return this.shapedWriter.Content; }
        }

        /// <summary>
        /// This returns the currently generated code the semantic routines have provided.
        /// </summary>
        public string MachineCode
        {
            get { return semanticRoutines.GeneratedCode; }
        }

        /// <summary>
        /// This is used to gather a more detailed set of actions that are taken by the compiler to create the 
        /// machine code output.
        /// </summary>
        public CompilerOutput CompilerParseActionList
        {
            get { return this.compilerOutput; }
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

            this.semanticRoutines = new SemanticRoutines(peekScanner, compilerOutput);

            SystemGoal();
        }

        private void SystemGoal()
        {
            this.shapedWriter.PushWrite("<SystemGoal>");
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call SystemGoal", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

            Program();
            Match("EofSym");

            semanticRoutines.Finish();

            this.shapedWriter.PopTab();
        }

        private void Program()
        {
            this.shapedWriter.PushWrite("<program>");
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call Program", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

            semanticRoutines.Start();
            
            Match("BeginSym");
            StatementList();
            Match("EndSym");

            this.shapedWriter.PopTab();
        }

        private void StatementList()
        {
            this.shapedWriter.PushWrite("<statement list>");
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call StatementList", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

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
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call Statement", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));


            ExprRecord identifier = ExprRecord.Create();
            ExprRecord expr = ExprRecord.Create();

            var nextToken = NextToken();
            switch (nextToken.Name)
            {
                case "Id":
                    Ident(identifier);
                    Match("AssignOp");
                    Expression(out expr);
                    semanticRoutines.Assign(identifier, expr);
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
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call IdList", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

            ExprRecord identifier = ExprRecord.Create();

            Ident(identifier);
            semanticRoutines.ReadId(identifier);
            if ("Comma".Equals(NextToken().Name, StringComparison.OrdinalIgnoreCase))
            {
                Match("Comma");
                IdList();
            }

            this.shapedWriter.PopTab();
        }

        private void Ident(ExprRecord result)
        {
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call Ident", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

            Match("Id");
            semanticRoutines.ProcessId(result);
        }

        private void ExprList()
        {
            this.shapedWriter.PushWrite("<expr list>");
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call ExpressionList", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

            ExprRecord expr = ExprRecord.Create();

            Expression(out expr);
            semanticRoutines.WriteExpr(expr);
            if ("Comma".Equals(NextToken().Name, StringComparison.OrdinalIgnoreCase))
            {
                Match("Comma");
                ExprList();
            }

            this.shapedWriter.PopTab();
        }

        private void Expression(out ExprRecord exprRec)
        {
            this.shapedWriter.PushWrite("<expression>");
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call Expression", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

            ExprRecord leftOperand = ExprRecord.Create(), rightOperand = ExprRecord.Create();
            OpRecord op = new OpRecord();
            
            Primary(leftOperand);
            var nextToken = NextToken();

            if (IsPlusOrMinus(nextToken))
            {
                AddOp(op);
                Expression(out rightOperand);
                exprRec = semanticRoutines.GenInfix(leftOperand, op, rightOperand);
            }
            else
            {
                exprRec = leftOperand;
            }

            this.shapedWriter.PopTab();
        }

        //private void Expression(out ExprRecord exprRec)
        //{
        //    this.shapedWriter.PushWrite("<expression>");

        //    ExprRecord leftOperand = ExprRecord.Create(), rightOperand = ExprRecord.Create();
        //    OpRecord op = new OpRecord();
        //    exprRec = leftOperand;
        //    Primary(leftOperand);
        //    var nextToken = NextToken();
        //    // use iteration instead of recursion...
        //    while (IsPlusOrMinus(nextToken))
        //    {
        //        AddOp(op);
        //        Expression(out rightOperand);
        //        exprRec = semanticRoutines.GenInfix(leftOperand, op, rightOperand);

        //        nextToken = NextToken();
        //    }


        //    //if ("PlusOp".Equals(nextToken.Name, StringComparison.OrdinalIgnoreCase))
        //    //{
        //    //    AddOp(op);
        //    //    Expression(out rightOperand);
        //    //    exprRec = semanticRoutines.GenInfix(leftOperand, op, rightOperand);
        //    //}
        //    //else if ("MinusOp".Equals(nextToken.Name, StringComparison.OrdinalIgnoreCase))
        //    //{
        //    //    AddOp(op);
        //    //    exprRec = semanticRoutines.GenInfix(leftOperand, op, rightOperand);
        //    //    Expression(out rightOperand);
        //    //}
        //    //else
        //    //{
        //    //    exprRec = leftOperand;
        //    //}

        //    this.shapedWriter.PopTab();
        //}

        private bool IsPlusOrMinus(Token nextToken)
        {
            return "PlusOp".Equals(nextToken.Name, StringComparison.OrdinalIgnoreCase) ||
                   "MinusOp".Equals(nextToken.Name, StringComparison.OrdinalIgnoreCase);
        }

        private void Primary(ExprRecord result)
        {
            this.shapedWriter.PushWrite("<primary>");
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call Primary", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

            var nextToken = NextToken();
            switch (nextToken.Name)
            {
                case "LParen":
                    Match("LParen");
                    Expression(out result);
                    Match("RParen");
                    break;
                case "Id":
                    Ident(result);
                    break;
                case "IntLiteral":
                    Match("IntLiteral");
                    semanticRoutines.ProcessLiteral(result);
                    break;
                default:
                    SyntaxError(nextToken);
                    break;
            }

            this.shapedWriter.PopTab();
        }

        private void AddOp(OpRecord op)
        {
            this.shapedWriter.PushWrite("<add op>");
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call AddOp", this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

            var nextToken = NextToken();
            switch (nextToken.Name)
            {
                case "PlusOp":
                    Match("PlusOp");
                    semanticRoutines.ProcessOp(op);
                    break;
                case "MinusOp":
                    Match("MinusOp");
                    semanticRoutines.ProcessOp(op);
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

            this.compilerOutput.AddRecord(ParseActionRecord.Create(string.Format("Call Match({0})", legalTokenName), this.peekScanner.Remaining, this.semanticRoutines.GeneratedCode));

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
    }
}
