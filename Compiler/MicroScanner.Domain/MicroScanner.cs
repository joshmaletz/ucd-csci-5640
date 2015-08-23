// <copyright file="MicroScanner.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroScanner.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// This is the implmentation of the Micro Scanner for our Ad-Hoc compiler. It follows the desired algorithm 
    /// quite closely.
    /// </summary>
    public class MicroScanner
    {
        /// <summary>
        /// The input source program
        /// </summary>
        private string program = string.Empty;

        /// <summary>
        /// The buffer to store characters as we are scanning.
        /// </summary>
        private Buffer buffer = new Buffer();

        /// <summary>
        /// The source program abstraction - allows us to inspect and advance.
        /// </summary>
        private SourceProgram sourceProgram = null;

        /// <summary>
        /// The output tokens
        /// </summary>
        private List<Token> outputTokens = new List<Token>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroScanner"/> class.
        /// </summary>
        /// <param name="program">The program.</param>
        public MicroScanner(string program)
        {
            this.program = program;
            this.sourceProgram = new SourceProgram(this.program);
        }

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public string Input
        {
            get
            {
                return this.program;
            }
        }

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public List<Token> Output
        {
            get
            {
                return new List<Token>(this.outputTokens);
            }
        }
        
        /// <summary>
        /// This is a helper method that just repeatedly calls scan on the source program until the End of file symbol 
        /// is encountered. As the tokens ae returned, we store them for use by callers.
        /// </summary>
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

        /// <summary>
        /// Scans the input to find the next valid token.
        /// </summary>
        /// <returns>The next valid token, if any. Will return the EOF token when at the end of the input.</returns>
        public Token Scan()
        {
            Token currentToken;
            if (this.sourceProgram.IsAtEnd())
            {
                currentToken = TokenFactory.CreateEndOfFileToken();
            }
            else
            {
                currentToken = GetNextToken();
                if (currentToken == null)
                {
                    currentToken = this.Scan();
                }
            }
            return currentToken;
        }

        /// <summary>
        /// Contains the logic needed to detect and return the next token in the source program.
        /// TODO: Refactor into helper methods and recursive functions. Use Character directly and not 'char'
        /// </summary>
        /// <returns>The next token in the source pogram.</returns>
        private Token GetNextToken()
        {
            // start with the next available character in the source program
            char nextChar = this.sourceProgram.ReadCurrentCharacter();
            var character = new Character(nextChar);
            
            // store internal buffer to make sharing any lexical errors easier.
            Buffer nextBuffer = new Buffer();
            nextBuffer.Add(nextChar);
            
            // create placeholder for token to be returned
            Token nextToken = null;
            
            // this could use some cleanup, but essentially we just perform the already discussed algorithm based on
            // the kind of character we encounter as we scan the file. Each case handled a different kind.
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

        /// <summary>
        /// Checks passed in token text to see if the value is that of a reserved word. 
        /// </summary>
        /// <param name="tokenText">The token text.</param>
        /// <returns>If it is a reserved word, this
        /// function returns a token for the reserved word; otherwise it just returns the passed in text as a 
        /// Literal (Id).</returns>
        private Token CheckReserved(string tokenText)
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
    }
}
