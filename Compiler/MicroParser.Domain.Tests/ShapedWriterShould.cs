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
        public void OutputWithNestedTabs()
        {
            var expectedOne = "\tHello";
            var expectedTwo = "\t\tWorld";

            
        }
    }
}
