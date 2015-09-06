// <copyright file="ExprKind.cs" company="Maletz, Josh" dateCreated="2015-09-06">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    /// <summary>
    /// This enum presents options for the kind of expressions which are allowed, and an 'unknown' used as a default before assignment.
    /// </summary>
    public enum ExprKind
    {
        IdExpr,
        LiteralExpr,
        TempExpr,
        Unknown
    }
}
