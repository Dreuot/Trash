using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems.BooleanOperations
{
    public class OperationOr : Operation<bool, bool>
    {
        public OperationOr(string name) : base(name)
        {
            Priority = 7;
            OperandsCount = 2;
        }
        public override Operand<bool> Execute(params Operand<bool>[] operands)
        {
            Operand<bool> result = new Constant<bool>("_temp", false);
            result.Value = operands[0].Value || operands[1].Value;

            return result;
        }
    }
}
