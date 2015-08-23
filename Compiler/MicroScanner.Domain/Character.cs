// <copyright file="Character.cs" company="Maletz, Josh" dateCreated="2015-08-22">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroScanner.Domain
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// The CharKind enumeration is used to explicitly identify the kind of character.
    /// </summary>
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

    /// <summary>
    /// The Character abstraction was created to allow for a more robust matching/determination of the kind of 
    /// character I was dealing with. the Built in 'char' type has some of this, but not everything I needed. I 
    /// decided to not even use the base functionality where it existed, and to implement all matches as regular 
    /// expressions.
    /// </summary>
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
}
