using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public class Constant : Operand
    {
        public Constant(string name, dynamic value) : base(name)
        {
            Value = value;
        }
    }
}
