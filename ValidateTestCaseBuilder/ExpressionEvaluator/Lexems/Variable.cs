using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public class Variable<T> : Operand<T>
    {
        public Variable(string name) : base(name)
        {
            Value = default(T);
        }
    }
}
