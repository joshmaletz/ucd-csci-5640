using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroScanner.Domain
{
    using System.Runtime.CompilerServices;
    using System.Runtime.Remoting.Messaging;
    using System.Text.RegularExpressions;

    public class Character
    {
        public char Value { get; private set; }
        public CharKind CharKind { get; private set; }
        public Character(char c)
        {
            this.Value = c;
            Identify();
        }

        private void Identify()
        {
            if (Regex.IsMatch(new string(new[] { this.Value }), @"^[(]$"))
            {
                this.CharKind = CharKind.LeftParen;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[)]$"))
            {
                this.CharKind = CharKind.RightParen;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[,]$"))
            {
                this.CharKind = CharKind.Comma;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[-]$"))
            {
                this.CharKind = CharKind.Hyphen;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[+]$"))
            {
                this.CharKind = CharKind.Plus;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[a-zA-Z_]$"))
            {
                this.CharKind = CharKind.Letter;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[\d]$"))
            {
                this.CharKind = CharKind.Digit;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[\s]$"))
            {
                this.CharKind = CharKind.Whitespace;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[:]$"))
            {
                this.CharKind = CharKind.Colon;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[;]$"))
            {
                this.CharKind = CharKind.Semicolon;
            }
            else if (Regex.IsMatch(new string(new[] { this.Value }), @"^[=]$"))
            {
                this.CharKind = CharKind.Equal;
            }
            else
            {
                this.CharKind = CharKind.Unknown;
            }

        }
    }

    public enum CharKind
    {
        Whitespace,
        LeftParen,
        RightParen,
        Plus,
        Semicolon,
        Comma,
        Colon,
        Equal,
        Digit,
        Letter,
        Hyphen,
        Unknown
    } 
}
