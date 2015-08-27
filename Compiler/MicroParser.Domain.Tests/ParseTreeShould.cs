using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroParser.Domain.Tests
{
    using NUnit.Framework;

    [TestFixture]   
    public class ParseTreeShould
    {
        [Test]
        public void AllowNestedOutputAtEachLevel()
        {
            var tree = new ParseTree("<system goal>");
            tree.AddChild("begin");
            tree.AddChild(BuildStatement());
            tree.AddChild("end");
            tree.BuildLines();
            Assert.AreEqual("<system goal>", tree.Lines[0]);
            Assert.AreEqual("begin <statement> end", tree.Lines[1]);
        }

        private TreeNode<string> BuildStatementNode()
        {
            return new TreeNode<string>("<statement>");
        }

        private string BuildStatement()
        {
            return "<statement>";
        }
    }
}
