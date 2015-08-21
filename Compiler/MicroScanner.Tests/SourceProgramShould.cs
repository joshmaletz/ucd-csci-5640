using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroScanner.Tests
{
    using MicroScanner.Domain;

    using NUnit.Framework;

    public class SourceProgramShould
    {
        [Test]
        public void AllowCreationWithEmptySource()
        {
            var source = new SourceProgram(string.Empty);

            Assert.IsNotNull(source);
        }

        [Test]
        public void ReturnEmptyCharWhenNoMoreSource()
        {
            var source = new SourceProgram(string.Empty);

            char next = source.Inspect();

            Assert.AreEqual('\0', next);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AdvanceOnEmptyShouldFail()
        {
            var source = new SourceProgram(string.Empty);
            
            source.Advance();
        }

        [Test]
        public void AdvanceShouldRemoveCharacter()
        {
            var source = new SourceProgram("Begin");

            for (int i = 0; i < 5; i++)
            {
                source.Advance();
            }

            Assert.AreEqual('\0',source.Inspect());
        }

        [Test]
        public void ReadCurrentCharShouldReturnNextCharAndAdvance()
        {
            var source = new SourceProgram("12");

            var next = source.ReadCurrentCharacter();
            Assert.AreEqual('1', next);

            next = source.ReadCurrentCharacter();
            Assert.AreEqual('2', next);
        }
    }
}
