using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroParser.Domain
{
    public class ShapedWriter
    {
        private string tabs = string.Empty;
        private StringBuilder outputBuilder = new StringBuilder();

        public string Content
        {
            get { return this.outputBuilder.ToString(); }
        }

        public void PushWrite(string message)
        {
            AddTab();
            this.outputBuilder.AppendLine(string.Format("{0}{1}", tabs, message));
        }

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
