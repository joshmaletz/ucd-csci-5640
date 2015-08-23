namespace MicroScanner.Domain
{
    using System;
    using System.Text;

    public class Buffer
    {
        private StringBuilder builder;

        public Buffer()
        {
            this.builder = new StringBuilder();
        }

        public void Add(char value)
        {
            this.builder.Append(value);
        }

        public void Clear()
        {
            this.builder.Clear();
        }

        public string Flush()
        {
            string currentValue = this.builder.ToString();
            this.Clear();
            return currentValue;
        }
    }
}
