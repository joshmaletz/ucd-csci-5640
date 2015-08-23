namespace MicroScanner.Tests
{
    using MicroScanner.Domain;
    using NUnit.Framework;

    [TestFixture]
    public class CharacterShould
    {
        [Test]
        public void MatchLeftParen()
        {
            var test = '(';
            var character = new Character(test);
            Assert.AreEqual(CharKind.LeftParen, character.CharKind);
        }

        [Test]
        public void MatchRightParen()
        {
            var test = ')';
            var character = new Character(test);
            Assert.AreEqual(CharKind.RightParen, character.CharKind);
        }

        [Test]
        public void MatchSpace()
        {
            var test = ' ';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Whitespace, character.CharKind);
        }

        [Test]
        public void MatchNewlineN()
        {
            var test = '\n';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Whitespace, character.CharKind);
        }

        [Test]
        public void MatchNewlineR()
        {
            var test = '\r';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Whitespace, character.CharKind);
        }

        [Test]
        public void MatchTab()
        {
            var test = '\t';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Whitespace, character.CharKind);
        }

        [Test]
        public void MatchLetter()
        {
            var test = 'g';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Letter, character.CharKind);
        }

        [Test]
        public void MatchDigit()
        {
            var test = '2';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Digit, character.CharKind);
        }

        [Test]
        public void MatchPlus()
        {
            var test = '+';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Plus, character.CharKind);
        }

        [Test]
        public void MatchHyphen()
        {
            var test = '-';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Hyphen, character.CharKind);
        }

        [Test]
        public void MatchComma()
        {
            var test = ',';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Comma, character.CharKind);
        }

        [Test]
        public void MatchSemicolon()
        {
            var test = ';';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Semicolon, character.CharKind);
        }

        [Test]
        public void MatchColon()
        {
            var test = ':';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Colon, character.CharKind);
        }

        [Test]
        public void MatchEqual()
        {
            var test = '=';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Equal, character.CharKind);
        }

        [Test]
        public void IdentifyUnknown()
        {
            var test = '>';
            var character = new Character(test);
            Assert.AreEqual(CharKind.Unknown, character.CharKind);
        }
    }
}
