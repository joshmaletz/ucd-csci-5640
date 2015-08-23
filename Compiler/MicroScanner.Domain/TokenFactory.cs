// <copyright file="TokenFactory.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroScanner.Domain
{
    /// <summary>
    /// This is a helper object I am using to return tokens. I decided to not create 'typed' tokens for this 
    /// implementation, so this factory only creates tokens where the names of the tokens are the only things that 
    /// differentiate them. May change in the future if it helps.
    /// </summary>
    public class TokenFactory
    {
        public static Token CreateEndOfFileToken()
        {
            return new Token("EofSym", "EOF");
        }

        public static Token CreateLeftParenToken()
        {
            return new Token("LParen", "(");
        }

        public static Token CreateRightParenToken()
        {
            return new Token("RParen", ")");
        }

        public static Token CreateSemiColonToken()
        {
            return new Token("SemiColon", ";");
        }

        public static Token CreatePlusToken()
        {
            return new Token("PlusOp", "+");
        }

        public static Token CreateCommaToken()
        {
            return new Token("Comma", ",");
        }

        public static Token CreateBeginToken()
        {
            return new Token("BeginSym", "BEGIN");
        }

        public static Token CreateEndToken()
        {
            return new Token("EndSym", "END");
        }

        public static Token CreateReadToken()
        {
            return new Token("ReadSym", "READ");
        }

        public static Token CreateWriteToken()
        {
            return new Token("WriteSym", "WRITE");
        }

        public static Token CreateAssignToken()
        {
            return new Token("AssignOp", ":=");
        }

        public static Token CreateMinusToken()
        {
            return new Token("MinusOp", "-");
        }

        public static Token CreateLiteralToken(string tokenText)
        {
            return new Token("Id", tokenText);
        }

        public static Token CreateIntLiteralToken(string tokenText)
        {
            return new Token("IntLiteral", tokenText);
        }

        public static Token CreateLexicalError(string tokenText)
        {
            return new Token("Lexical Error", string.Format("Bad char value: '{0}'", tokenText));
        }
    }
}
