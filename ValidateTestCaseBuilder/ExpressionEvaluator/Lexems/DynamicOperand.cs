using ExpressionEvaluatorLib.Lexems;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    class DynamicOperand : Lexem
    {
        public dynamic Value { get; set; }

        public DynamicOperand(string name) : base(name)
        {
            Type = LexemType.Operand;
        }
    }
}
