// <copyright file="SemanticRoutines.cs" company="Maletz, Josh" dateCreated="2015-09-06">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Probably wouldn't normally create this class, as I am using it as a wrapper to store the [majority of] new functionlaity for this assignment.
    /// </summary>
    public class SemanticRoutines
    {
        private MicroGenerator generator = null;
        private int maxSymbol;
        private int maxTemp;
        private Dictionary<string, string> symbolTable  = new Dictionary<string, string>();
        private PeekableScanner scanner = null;
        private CompilerOutput compilerOutput = null;

        /// <summary>
        /// Returns all generated code
        /// </summary>
        public string GeneratedCode
        {
            get { return this.generator.ToString();  }
        }

        /// <summary>
        /// Creates a semantic routines object, giving it access to the scanner and output logger
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="compilerOutput"></param>
        public SemanticRoutines(PeekableScanner scanner, CompilerOutput compilerOutput)
        {
            // Createds new generator on startup
            this.generator = new MicroGenerator();
            this.scanner = scanner;
            this.compilerOutput = compilerOutput;
        }

        /// <summary>
        /// Start routine
        /// </summary>
        public void Start()
        {
            this.maxSymbol = 0;
            this.maxTemp = 0;

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call Start", this.scanner.Remaining, this.generator.ToString()));
        }

        /// <summary>
        /// Assign routine
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public void Assign(ExprRecord target, ExprRecord source)
        {
            this.generator.Generate("Store", Extract(source), target.Name);

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call Assign", this.scanner.Remaining, this.generator.ToString()));
        }

        /// <summary>
        /// ReadId routine
        /// </summary>
        /// <param name="inVar"></param>
        public void ReadId(ExprRecord inVar)
        {
            this.generator.Generate("Read", inVar.Name, "Integer");

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call ReadId", this.scanner.Remaining, this.generator.ToString()));
        }

        /// <summary>
        /// CheckId helper routine
        /// </summary>
        /// <param name="tokenName"></param>
        private void CheckId(string tokenName)
        {
            if (!Lookup(tokenName))
            {
                Enter(tokenName);
                this.generator.Generate("Declare", tokenName, "Integer");
            }

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call CheckId", this.scanner.Remaining, this.generator.ToString()));
        }

        /// <summary>
        /// Checks teh symbol table to see if token is already there.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        private bool Lookup(string tokenName)
        {
            return this.symbolTable.ContainsKey(tokenName);
        }

        /// <summary>
        /// Enters token symbol into symbol table
        /// </summary>
        /// <param name="tokenName"></param>
        private void Enter(string tokenName)
        {
            this.symbolTable.Add(tokenName, tokenName);
        }


        /// <summary>
        /// Finish routine
        /// </summary>
        public void Finish()
        {
            this.generator.Generate("Halt");

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call Finish", this.scanner.Remaining, this.generator.ToString()));
        }

        /// <summary>
        /// Extract helper method - determines what part of hte record should be returned to the generator.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public string Extract(ExprRecord expr)
        {
            string output = string.Empty;
            switch (expr.Kind)
            {
                case ExprKind.IdExpr:
                case ExprKind.TempExpr:
                    output = expr.Name;
                    break;
                case ExprKind.LiteralExpr:
                    output = expr.Value.ToString();
                    break;
            }
            return output;
        }

        /// <summary>
        /// Determines what command value should be returned to the generator for the operation.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public string ExtractOp(OpRecord op)
        {
            return op.Op == "+" ? "ADD" : "SUB";
        }

        /// <summary>
        /// Process Id routine
        /// </summary>
        /// <param name="e"></param>
        public void ProcessId(ExprRecord e)
        {
            string tokenBufferValue = this.scanner.TokenBuffer();
            CheckId(tokenBufferValue); 
            e.Kind = ExprKind.IdExpr;
            e.Name = tokenBufferValue;

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call ProcessId", this.scanner.Remaining, this.generator.ToString()));
        }

        /// <summary>
        /// Process Literal routine
        /// </summary>
        /// <param name="e"></param>
        public void ProcessLiteral(ExprRecord e)
        {
            string tokenBufferValue = this.scanner.TokenBuffer();

            e.Kind = ExprKind.LiteralExpr;
            e.Value = ConvertToInteger(tokenBufferValue);

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call ProcessLiteral", this.scanner.Remaining, this.generator.ToString()));
        }

        /// <summary>
        /// Process Op routine
        /// </summary>
        /// <param name="o"></param>
        public void ProcessOp(OpRecord o)
        {
            string currentToken = this.scanner.TokenBuffer(); // value will be peekable scanner token value...
            o.Op = currentToken;

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call ProcessOp", this.scanner.Remaining, this.generator.ToString()));
        }

        private long ConvertToInteger(string valueAsString)
        {
            long value;
            bool parsed = long.TryParse(valueAsString, out value);
            if (!parsed)
            {
                throw new SyntaxErrorException(
                    string.Format("Tried to parse an integer, but value was not an integer! Value in question: {0}",
                        valueAsString));
            }

            return value;
        }
        
        /// <summary>
        /// Write Expr routine
        /// </summary>
        /// <param name="outExpr"></param>
        public void WriteExpr(ExprRecord outExpr)
        {
            this.generator.Generate("Write", Extract(outExpr), "Integer");

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call WriteExpr", this.scanner.Remaining, this.generator.ToString()));
        }

        /// <summary>
        /// Gen Infix routine
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="op"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public ExprRecord GenInfix(ExprRecord e1, OpRecord op, ExprRecord e2)
        {
            var eRec = ExprRecord.CreateForTemp();
            eRec.Name = GetTemp();
            this.generator.Generate(ExtractOp(op), Extract(e1), Extract(e2), eRec.Name);

            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call GenInfix", this.scanner.Remaining, this.generator.ToString()));

            return eRec;
        }

        /// <summary>
        /// Get Temp helper routine. Creates a temp and calls check id to makse sure we can use it.
        /// </summary>
        /// <returns></returns>
        private string GetTemp()
        {
            this.compilerOutput.AddRecord(ParseActionRecord.Create("Call GetTemp", this.scanner.Remaining, this.generator.ToString()));
            
            this.maxTemp++;
            string tempVar = string.Format("Temp&{0}", this.maxTemp);
            CheckId(tempVar);
            return tempVar;
        }
    }
}