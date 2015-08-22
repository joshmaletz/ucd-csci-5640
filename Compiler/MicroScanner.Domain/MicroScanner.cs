

namespace MicroScanner.Domain
{
    using System.Collections.Generic;

    public class MicroScanner
    {
        private string program = string.Empty;
        private Buffer buffer = new Buffer();
        private SourceProgram sourceProgram = null;
        private List<Token> outputTokens = new List<Token>(); 

        public string Input
        {
            get
            {
                return this.program;
            }
        }

        public List<Token> Output
        {
            get
            {
                return new List<Token>(this.outputTokens);
            }
        } 

        public MicroScanner(string program)
        {
            this.program = program;
            this.sourceProgram = new SourceProgram(this.program);
        }

        public Token Scan()
        {
            Token currentToken;
            if (this.sourceProgram.IsAtEnd())
            {
                currentToken = TokenFactory.CreateEndOfFileToken();
            }
            else
            {
                currentToken = GetNextToken(this.program);
                if (currentToken == null)
                {
                    currentToken = this.Scan();
                }
            }
            return currentToken;
        }

        public Token GetNextToken(string currentProgram)
        {
            char nextChar = this.sourceProgram.ReadCurrentCharacter();
            var character = new Character(nextChar);
            Buffer nextBuffer = new Buffer();
            nextBuffer.Add(nextChar);
            Token nextToken = null;
            switch (character.CharKind)
            {
               case CharKind.Letter:
                    this.buffer.Add(character.Value);
                    char nextUp = this.sourceProgram.Inspect();
                    var nextUpCharacter = new Character(nextUp);
                    while (CharKind.Letter.Equals(nextUpCharacter.CharKind) || CharKind.Digit.Equals(nextUpCharacter.CharKind))
                    {
                        this.buffer.Add(nextUpCharacter.Value);
                        this.sourceProgram.Advance();

                        nextUp = this.sourceProgram.Inspect();
                        nextUpCharacter = new Character(nextUp);
                    }
                    nextToken = CheckReserved(this.buffer.Flush());
                    break;
                case CharKind.Digit:
                    this.buffer.Add(character.Value);
                    char nextUpD = this.sourceProgram.Inspect();
                    var nextUpCharacterD = new Character(nextUpD);
                    while (CharKind.Digit.Equals(nextUpCharacterD.CharKind))
                    {
                        this.buffer.Add(nextUpCharacterD.Value);
                        this.sourceProgram.Advance();

                        nextUpD = this.sourceProgram.Inspect();
                        nextUpCharacterD = new Character(nextUpD);
                    }
                    nextToken = TokenFactory.CreateIntLiteralToken(this.buffer.Flush());
                    break;
                case CharKind.Colon:
                    //this.buffer.Add(character.Value);
                    char nextUpE = this.sourceProgram.Inspect();
                    var nextUpCharacterE = new Character(nextUpE);
                    if (nextUpCharacterE.CharKind.Equals(CharKind.Equal))
                    {
                        this.sourceProgram.Advance();
                        nextToken = TokenFactory.CreateAssignToken();
                    }
                    else
                    {
                        nextBuffer.Add(nextUpCharacterE.Value);
                        nextToken = TokenFactory.CreateLexicalError(nextBuffer.Flush());
                    }
                    break;
                case CharKind.Hyphen:
                    char nextUpH = this.sourceProgram.Inspect();
                    var nextUpCharacterH = new Character(nextUpH);
                    if (nextUpCharacterH.CharKind.Equals(CharKind.Hyphen))
                    {
                        // comment - so advance until end of line
                        char inCommentChar = this.sourceProgram.ReadCurrentCharacter();
                        while (!inCommentChar.Equals('\n'))
                        {
                            inCommentChar = this.sourceProgram.ReadCurrentCharacter();
                        }
                    }
                    else
                    {
                        nextToken = TokenFactory.CreateMinusToken();
                    }
                    break;
               case CharKind.Whitespace:
                    break;
               case CharKind.LeftParen:
                    nextToken = TokenFactory.CreateLeftParenToken();
                    break;
                case CharKind.RightParen:
                    nextToken = TokenFactory.CreateRightParenToken();
                    break;
                case CharKind.Semicolon:
                    nextToken = TokenFactory.CreateSemiColonToken();
                    break;
                case CharKind.Comma:
                    nextToken = TokenFactory.CreateCommaToken();
                    break;
                case CharKind.Plus:
                    nextToken = TokenFactory.CreatePlusToken();
                    break;
                default:
                    nextToken = TokenFactory.CreateLexicalError(nextBuffer.Flush());
                    break;
            }

            return nextToken;
        }

        public Token CheckReserved(string tokenText)
        {
            Token token = null;
            switch (tokenText.ToUpper())
            {
                case "BEGIN":
                    token = TokenFactory.CreateBeginToken();
                    break;
                case "END":
                    token = TokenFactory.CreateEndToken();
                    break;
                case "WRITE":
                    token = TokenFactory.CreateWriteToken();
                    break;
                case "READ":
                    token = TokenFactory.CreateReadToken();
                    break;
                default:
                    token = TokenFactory.CreateLiteralToken(tokenText);
                    break;
            }

            return token;
        }

        public void ScanAll()
        {
            Token currentToken = null;
            do
            {
                currentToken = this.Scan();
                this.outputTokens.Add(currentToken);
            }
            while (currentToken != null && !"EofSym".Equals(currentToken.Name));
        }
    }
}
