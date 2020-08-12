using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems.CompareOperations
{
    class OperationEqual : Operation<IComparable, bool>
    {
        public OperationEqual(string name) : base(name)
        {
            Priority = 0;
            OperandsCount = 2;
        }

        public override Operand<bool> Execute(params Operand<IComparable>[] operands)
        {
            Operand<bool> result = new Constant<bool>("_temp", default(bool));
            result.Value = operands[0].Value.CompareTo(operands[1].Value) == 0;

            return result;
        }
    }
}
