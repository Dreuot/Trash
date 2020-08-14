using ExpressionEvaluatorLib.Lexems;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public class Operand : Lexem
    {
        public dynamic Value { get; set; }

        public Operand(string name) : base(name)
        {
            Type = LexemType.Operand;
        }
    }
}
