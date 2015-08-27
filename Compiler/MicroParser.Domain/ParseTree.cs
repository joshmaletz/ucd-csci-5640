using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroParser.Domain
{
    public class ParseTree : TreeNode<string>
    {
        public List<string> Lines = new List<string>();

        public ParseTree(string root)
            : base(root)
        {
            
        }

        public void BuildLinesDifferently()
        {
            this.Lines.Add(this.Item);
            this.Lines.Add(string.Format(GetChildLine(this)));
            this.Template = "{0}";
            EveryNonTermChildHasOneLine(this.Template, this);
        }

        private string GetChildLine(TreeNode<string> node)
        {
            string line = string.Empty;
            foreach (var child in node.Children)
            {
                line += child.Item;
                line += " ";
            }

            return line;
        }

        private void EveryNonTermChildHasOneLine(string template, TreeNode<string> node)
        {
            string line = GetChildLine(node);
            
            // for every non term child of the node, create one line
            //this.Lines.Add(string.Format(template, line));

           

            // now line as is should be added.
            List<string> linesToGenerate = new List<string>();
            Dictionary<int, int> nonTerminalMap = new Dictionary<int, int>();
            int countOfNonTerminalsInChildren = 0;
            int index = 0;
            foreach (var child in node.Children)
            {
                if (!child.IsTerminal)
                {
                    nonTerminalMap.Add(countOfNonTerminalsInChildren, index);
                    countOfNonTerminalsInChildren++;
                }

                index++;
            }

            for (int nonTerminalIndex = 0; nonTerminalIndex < countOfNonTerminalsInChildren; nonTerminalIndex++)
            {
                string nextLineTemplate = string.Empty;

                // build template lines 1 at a time..
                line = string.Empty;
                string childLine = string.Empty;
                
                for(int childIndex = 0; childIndex < node.Children.Count; childIndex++)
                //foreach (var child in node.Children)
                {
                    var child = node.Children[childIndex];
                    if (child.IsTerminal)
                    {
                        nextLineTemplate += child.Item;
                        nextLineTemplate += " ";
                    }
                    else if (!child.IsTerminal && child.HasChildren())
                    {
                        var currentNonTerminal = nonTerminalMap[nonTerminalIndex];
                        if (childIndex == currentNonTerminal)
                        {
                            //countOfSubs++;
                            //digDeeper = true;
                            nextLineTemplate += "{0}";
                            //nextLineTemplate += " ";
                            childLine = GetChildLine(child);
                            //AddLine(child, nextLineTemplate);
                        }
                        else
                        {
                            if (child.HasBeenHandled)
                            {
                                nextLineTemplate += GetChildLine(child);
                                //nextLineTemplate += " ";
                            }
                            else
                            {
                                nextLineTemplate += child.Item;
                                nextLineTemplate += " ";
                            }
                        }
                    }
                }

                //string wholeTemplate = template;
                string wholeTemplate = string.Format(template, nextLineTemplate);
                //string childLine = GetChildLine(node);
                this.Lines.Add(string.Format(wholeTemplate, childLine));

                node.Children[nonTerminalMap[nonTerminalIndex]].Template = wholeTemplate;
                node.Children[nonTerminalMap[nonTerminalIndex]].HasBeenHandled = true;
            }

            foreach (var child in node.Children)
            {
                if (!child.IsTerminal)
                {
                    EveryNonTermChildHasOneLine(child.Template, child);
                }
            }
        }

        public void BuildLines()
        {
            this.Lines.Add(this.Item);
            
            // build a recursive output by walking the tree.
            // each line should build by all outer nodes included in output

            AddLine(this, "{0}");
        }

        private void AddLine(TreeNode<string> node, string template)
        {
            string line = string.Empty;
            string nextLineTemplate = string.Empty;
            bool digDeeper = false;
            int countOfSubs = 0;
            foreach (var child in node.Children)
            {
                line += child.Item;
                line += " ";
                if (child.IsTerminal)
                {
                    nextLineTemplate += child.Item;
                    nextLineTemplate += " ";
                }
                else if(!child.IsTerminal && child.HasChildren())
                {
                    if (countOfSubs == 0)
                    {
                        countOfSubs++;
                        digDeeper = true;
                        nextLineTemplate += "{0}";
                        //nextLineTemplate += " ";
                        //AddLine(child, nextLineTemplate);
                    }
                    else
                    {
                        nextLineTemplate += child.Item;
                        nextLineTemplate += " ";
                    }
                }
            }

            this.Lines.Add(string.Format(template, line));

            // now we should have created next line template if there was a non terminal
            if (digDeeper == true)
            {
                foreach (var child in node.Children)
                {
                    if (!child.IsTerminal && child.HasChildren())
                    {
                        AddLine(child, string.Format(template, nextLineTemplate));
                    }
                }
            }


            
        }

        
    }

    public class TreeNode<T>
    {
        public List<TreeNode<T>> Children;

        public void AddChildNode(TreeNode<T> buildStatementNode)
        {
            this.Children.Add(buildStatementNode);
        }

        public T Item { get; set; }
        public string Template { get; set; }
        public bool HasBeenHandled { get; set; }
        public TreeNode(T item)
        {
            Item = item;
            Children = new List<TreeNode<T>>();
        }

        public bool IsTerminal { get; private set; }

        public TreeNode<T> AddTerminalChild(T item)
        {
            
            var nodeItem = this.AddChild(item);
            nodeItem.IsTerminal = true;

            return nodeItem;
        } 

        public TreeNode<T> AddChild(T item)
        {
            TreeNode<T> nodeItem = new TreeNode<T>(item);
            Children.Add(nodeItem);
            return nodeItem;
        }

        public bool HasChildren()
        {
            return Children.Count > 0;
        }
    }
}
