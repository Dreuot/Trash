using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public class Variable : Operand
    {
        public Variable(string name) : base(name)
        {
        }
    }
}
