using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public abstract class Operation<T, U> : Lexem
    {
        public int Priority { get; protected set; }
        public int OperandsCount { get; protected set; }
        public Operation(string name) : base(name)
        {
        }

        public abstract Operand<U> Execute(params Operand<T>[] operands);
    }
}
