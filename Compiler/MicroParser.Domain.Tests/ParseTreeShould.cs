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
        public ParseTreeShould()
        {
        }

        [Test]
        public void AllowNestedOutputAtEachLevel2()
        {
            var tree = new ParseTree("<system goal>");
            tree.AddTerminalChild("begin");
            //tree.AddChild(BuildStatement());
            tree.AddChildNode(BuildStatementNode());
            tree.AddTerminalChild("end");
            tree.BuildLinesDifferently();
            Assert.AreEqual("<system goal>", tree.Lines[0]);
            Assert.AreEqual("begin <statement> end ", tree.Lines[1]);
            Assert.AreEqual("begin Id := <expression> ; end ", tree.Lines[2]);
            Assert.AreEqual("begin Id := <primary> <add op> <expression> ; end ", tree.Lines[3]);
            Assert.AreEqual("begin Id := Id <add op> <expression> ; end ", tree.Lines[4]);
            Assert.AreEqual("begin Id := Id PlusOp <expression> ; end ", tree.Lines[5]);
            Assert.AreEqual("begin Id := Id PlusOp <primary> ; end ", tree.Lines[6]);
            Assert.AreEqual("begin Id := Id PlusOp IdX ; end ", tree.Lines[7]);
        }

        [Test]
        public void AllowNestedOutputAtEachLevel()
        {
            var tree = new ParseTree("<system goal>");
            tree.AddTerminalChild("begin");
            //tree.AddChild(BuildStatement());
            tree.AddChildNode(BuildStatementNode());
            tree.AddTerminalChild("end");
            tree.BuildLines();
            Assert.AreEqual("<system goal>", tree.Lines[0]);
            Assert.AreEqual("begin <statement> end ", tree.Lines[1]);
            Assert.AreEqual("begin Id := <expression> ; end ", tree.Lines[2]);
            Assert.AreEqual("begin Id := <primary> <add op> <expression> ; end ", tree.Lines[3]);
            Assert.AreEqual("begin Id := Id <add op> <expression> ; end ", tree.Lines[4]);
            Assert.AreEqual("begin Id := Id PlusOp <expression> ; end ", tree.Lines[5]);
            Assert.AreEqual("begin Id := Id PlusOp <primary> ; end ", tree.Lines[6]);
            Assert.AreEqual("begin Id := Id PlusOp IdX ; end ", tree.Lines[7]);
        }

        private TreeNode<string> BuildStatementNode()
        {
            var statementNode = new TreeNode<string>("<statement>");
            statementNode.AddTerminalChild("Id");
            statementNode.AddTerminalChild(":=");
            statementNode.AddChildNode(BuildExpressionNode());
            statementNode.AddTerminalChild(";");
            return statementNode;
        }

        private TreeNode<string> BuildExpressionNode()
        {
            var expressionNode = new TreeNode<string>("<expression>");

            expressionNode.AddChildNode(BuildFirstPrimaryIdNode());
            expressionNode.AddChildNode(BuildAddOpNode());
            expressionNode.AddChildNode(BuildPrimaryExpression());

            return expressionNode;
        }

        private TreeNode<string> BuildPrimaryExpression()
        {
            var firstPrimary = new TreeNode<string>("<expression>");
            var terminalPrimary = new TreeNode<string>("<primary>");
            terminalPrimary.AddTerminalChild("IdX");
            firstPrimary.AddChildNode(terminalPrimary);

            return firstPrimary;
        }

        private TreeNode<string> BuildAddOpNode()
        {
            var addOp = new TreeNode<string>("<add op>");

            addOp.AddTerminalChild("PlusOp");

            return addOp;
        } 

        private TreeNode<string> BuildFirstPrimaryIdNode()
        {
            var firstPrimary = new TreeNode<string>("<primary>");

            firstPrimary.AddTerminalChild("Id");

            return firstPrimary;
        } 

        private string BuildStatement()
        {
            return "<statement>";
        }
    }
}
