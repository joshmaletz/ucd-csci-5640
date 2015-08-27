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

        public void BuildLines()
        {
            this.Lines.Add(this.Item);
            
            // build a recursive output by walking the tree.
            // each line should build by all outer nodes included in output

            AddLine(this);
        }

        private void AddLine(TreeNode<string> node)
        {
            string line = string.Empty;
            foreach (var child in node.Children)
            {
                line += child.Item;
                line += " ";
                //if (!child.HasChildren())
                //{

                //}
                //else
                //{

                //}
            }
            this.Lines.Add(line);
        }
    }

    public class TreeNode<T>
    {
        public List<TreeNode<T>> Children;

        public T Item { get; set; }

        public TreeNode(T item)
        {
            Item = item;
            Children = new List<TreeNode<T>>();
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
