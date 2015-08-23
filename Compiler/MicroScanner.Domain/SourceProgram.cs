namespace MicroScanner.Domain
{
    using System;

    public class SourceProgram
    {
        private string sourceProgram = string.Empty;

        public SourceProgram(string input)
        {
            this.sourceProgram = input;
        }

        public void Advance()
        {
            if (this.sourceProgram.Length <= 0)
            {
                throw new InvalidOperationException("No more source to advance.");
            }
           
            this.sourceProgram = this.sourceProgram.Substring(1, this.sourceProgram.Length - 1);
        }

        public char Inspect()
        {
            char nextChar = char.MinValue;
            if (this.sourceProgram.Length > 0)
            {
                nextChar = this.sourceProgram[0];
            }

            return nextChar;
        }

        public char ReadCurrentCharacter()
        {
            char nextChar = char.MinValue;
            if (this.sourceProgram.Length > 0)
            {
                nextChar = this.sourceProgram[0];
                this.sourceProgram = this.sourceProgram.Substring(1, this.sourceProgram.Length - 1);
            }            

            return nextChar;
        }

        public bool IsAtEnd()
        {
            return 0.Equals(this.sourceProgram.Length);
        }
    }
}
