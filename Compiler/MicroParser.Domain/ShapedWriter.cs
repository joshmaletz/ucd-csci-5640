// <copyright file="ShapedWriter.cs" company="Maletz, Josh" dateCreated="2015-08-28">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    using System.Text;

    /// <summary>
    /// The ShapedWriter class is helper utility to create a structured output. It acts like a stack 
    /// to push and pop indentation levels while writing the identified symbols to the output stream. 
    /// It just makes it a bit easier to see the transformations. I would rather use this to operate on
    /// a parse tree, rather than build this during the algortihm.
    /// </summary>
    public class ShapedWriter
    {
        private string tabs = string.Empty;
        private StringBuilder outputBuilder = new StringBuilder();

        public string Content
        {
            get { return this.outputBuilder.ToString(); }
        }

        /// <summary>
        /// Add a 'tab' to the indentation tracker and write the message to the output.
        /// </summary>
        /// <param name="message"></param>
        public void PushWrite(string message)
        {
            AddTab();
            this.outputBuilder.AppendLine(string.Format("{0}{1}", tabs, message));
        }

        /// <summary>
        /// Remove a 'tab' from the indentation tracker.
        /// </summary>
        public void PopTab()
        {
            if (this.tabs.Length >= 3)
            {
                this.tabs = this.tabs.Remove(tabs.Length - 3, 3);
            }
        }

        private void AddTab()
        {
            this.tabs += "   ";
        }
    }
}
