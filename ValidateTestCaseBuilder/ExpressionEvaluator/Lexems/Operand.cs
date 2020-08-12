using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public class Operand<T> : Lexem
    {
        public T Value { get; set; }

        public Operand(string name) : base(name)
        {
            Value = default(T);
        }
    }
}
