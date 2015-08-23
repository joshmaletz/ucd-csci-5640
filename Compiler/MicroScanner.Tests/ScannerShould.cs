// <copyright file="ScannerShould.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroScanner.Tests
{
    using MicroScanner.Domain;
    using NUnit.Framework;

    public class ScannerShould
    {
        [Test]
        public void AllowEmptyProgramAtCreation()
        {
            var microScanner = new MicroScanner(string.Empty);
            Assert.IsNotNull(microScanner);
            Assert.AreEqual(string.Empty, microScanner.Input);
        }

        [Test]
        public void TurnEmptyProgamIntoEofSymToken()
        {
            var program = string.Empty;
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("EofSym", token.Name);
        }

        [Test]
        public void DetectAndIgnoreWhitespaceSpaces()
        {
            var program = " ";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("EofSym", token.Name);
        }

        [Test]
        public void DetectAndIgnoreWhitespaceTabs()
        {
            var program = "\t";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("EofSym", token.Name);
        }

        [Test]
        public void DetectAndIgnoreWhitespaceNewlinesN()
        {
            var program = "\n";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("EofSym", token.Name);
        }

        [Test]
        public void DetectAndIgnoreWhitespaceNewLinesR()
        {
            var program = "\r";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("EofSym", token.Name);
        }

        [Test]
        public void AllowCreationWithProgramAsString()
        {
            string program = "Begin End";
            var scanner = new MicroScanner(program);
            Assert.AreEqual(program, scanner.Input);
        }

        [Test]
        public void ReturnTokenWhenScanIsCalled()
        {
            string program = "Begin";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("BeginSym", token.Name);
        }

        [Test]
        public void DetectAndReturnBeginSymbol()
        {
            string program = "Begin";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("BeginSym", token.Name);
            Assert.AreEqual("BEGIN", token.Value);
        }

        [Test]
        public void DetectAndReturnReadReservedWord()
        {
            string program = "Read";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("ReadSym", token.Name);
            Assert.AreEqual("READ", token.Value);
        }

        [Test]
        public void DetectAndReturnWriteReservedWord()
        {
            string program = "Write";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("WriteSym", token.Name);
            Assert.AreEqual("WRITE", token.Value);
        }

        [Test]
        public void DetectAndReturnEndReservedWord()
        {
            string program = "End";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("EndSym", token.Name);
            Assert.AreEqual("END", token.Value);
        }

        [Test]
        public void DetectAndReturnLiteral()
        {
            string program = "abcdefghijklmnopqrstuvwxyz0123456789_ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("Id", token.Name);
            Assert.AreEqual(program, token.Value);
        }

        [Test]
        public void ReturnLexicalErrorWhenBadCharactersEncountered()
        {
            string program = "First<Second";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("Id", token.Name);
            Assert.AreEqual("First", token.Value);
            
            token = microScanner.Scan();
            Assert.AreEqual("Lexical Error", token.Name, token.Value);
            Assert.AreEqual("Bad char value: '<'", token.Value);

            token = microScanner.Scan();
            Assert.AreEqual("Id", token.Name);
            Assert.AreEqual("Second", token.Value);
        }

        [Test]
        public void DetectAndReturnMinusOp()
        {
            string program = "-";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("MinusOp", token.Name);
        }

        [Test]
        public void DetectAndIgnoreCommentLine()
        {
            string program = "-- Some comment 12345\r\n ";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("EofSym", token.Name);
            Assert.AreEqual("EOF", token.Value);
        }

        [Test]
        public void DetectAndReturnAssignOp()
        {
            string program = ":=";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("AssignOp", token.Name);
            Assert.AreEqual(":=", token.Value);
        }

        [Test]
        public void DetectColonFollwedByAnythingButEqualAsError()
        {
            string program = ":a";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("Lexical Error", token.Name);
            Assert.AreEqual("Bad char value: ':a'", token.Value);
        }

        [Test]
        public void DetectSingleColonAsError()
        {
            string program = ":";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("Lexical Error", token.Name);   
        }

        [Test]
        public void DetectSingleEqualAsError()
        {
            string program = "=";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("Lexical Error", token.Name);
        }

        [Test]
        public void DetectAndReturnLeftParen()
        {
            string program = "(";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("LParen", token.Name);
        }

        [Test]
        public void DetectAndReturnRightParen()
        {
            string program = ")";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("RParen", token.Name);
        }

        [Test]
        public void DetectAndReturnSemiColon()
        {
            string program = ";";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("SemiColon", token.Name);
        }

        [Test]
        public void DetectAndReturnComma()
        {
            string program = ",";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("Comma", token.Name);
        }

        [Test]
        public void DetectAndReturnPlusSymbol()
        {
            string program = "+";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("PlusOp", token.Name);
        }

        [Test]
        public void DetectAndReturnIntLiteral()
        {
            string program = "1234";
            var microScanner = new MicroScanner(program);

            var token = microScanner.Scan();
            Assert.AreEqual("IntLiteral", token.Name);
            Assert.AreEqual("1234", token.Value);
        }

        [Test]
        public void AlwaysAppendEndOfFileTokenWhileScanning()
        {
            string program = "Begin";
            var microScanner = new MicroScanner(program);
            microScanner.ScanAll();
            var listOfTokens = microScanner.Output;
            Assert.AreEqual(2, listOfTokens.Count);
            Assert.AreEqual("BeginSym", listOfTokens[0].Name);
            Assert.AreEqual("EofSym", listOfTokens[1].Name);
        }
    }
}
