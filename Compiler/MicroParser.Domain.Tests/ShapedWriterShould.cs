using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroParser.Domain.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ShapedWriterShould
    {
        [Test]
        public void PushOneTabWithMessageAsSingleLine()
        {
            var shapedWriter = new ShapedWriter();
            shapedWriter.PushWrite("Hello");

            var expected = "   Hello\r\n";
            Assert.AreEqual(expected, shapedWriter.Content);
        }

        [Test]
        public void PushMultipleTabs()
        {
            var shapedWriter = new ShapedWriter();
            shapedWriter.PushWrite("Hello");
            shapedWriter.PushWrite("World");

            var expected = "   Hello\r\n      World\r\n";
            Assert.AreEqual(expected, shapedWriter.Content);
        }

        [Test]
        public void PopOneTabWhenAsked()
        {
            var shapedWriter = new ShapedWriter();
            shapedWriter.PushWrite("Hello");
            shapedWriter.PushWrite("World");
            shapedWriter.PopTab();
            shapedWriter.PushWrite("Again");
            
            var expected = "   Hello\r\n      World\r\n      Again\r\n";
            Assert.AreEqual(expected, shapedWriter.Content);
        }

        [Test]
        public void PopTwosTabWhenAsked()
        {
            var shapedWriter = new ShapedWriter();
            shapedWriter.PushWrite("Hello");
            shapedWriter.PushWrite("World");
            shapedWriter.PopTab();
            shapedWriter.PushWrite("Again");
            shapedWriter.PopTab();
            shapedWriter.PopTab();
            shapedWriter.PushWrite("Good-bye");

            var expected = "   Hello\r\n      World\r\n      Again\r\n   Good-bye\r\n";
            Assert.AreEqual(expected, shapedWriter.Content);
        }
    }
}
