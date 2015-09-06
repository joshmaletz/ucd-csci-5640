// <copyright file="ExprRecord.cs" company="Maletz, Josh" dateCreated="2015-09-06">
//      Copyright 2015 Maletz, Josh- For eductional purposes. Created while student of UCD CSCI 5640 - Universal Compiler.
// </copyright>

namespace MicroParser.Domain
{
    /// <summary>
    /// Abstraction for the ExpressionRecord type used to generate code. 
    /// </summary>
    public class ExprRecord
    {
        public ExprKind Kind { get; set; }
        public string Name { get; set; }
        public long Value { get; set; }

        private ExprRecord()
        {
            
        }

        /// <summary>
        /// Public factory method to be used for a temp variable
        /// </summary>
        /// <returns></returns>
        public static ExprRecord CreateForTemp()
        {
            return new ExprRecord()
            {
                Kind = ExprKind.TempExpr
            };
        }

        /// <summary>
        /// public factory method to allow default instantiation.
        /// </summary>
        /// <returns></returns>
        public static ExprRecord Create()
        {
            return new ExprRecord()
            {
                Kind = ExprKind.Unknown
            };
        }
    }
}
