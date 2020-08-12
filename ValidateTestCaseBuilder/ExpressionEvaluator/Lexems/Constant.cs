using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public class Constant<T> : Operand<T>
    {
        public Constant(string name, T value) : base(name)
        {
            Value = value;
        }
    }
}
