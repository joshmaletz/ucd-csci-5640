using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroScanner.Domain
{
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
