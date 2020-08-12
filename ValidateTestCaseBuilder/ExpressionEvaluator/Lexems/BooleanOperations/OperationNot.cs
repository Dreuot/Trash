using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems.BooleanOperations
{
    public class OperationNot : Operation<bool, bool>
    {
        public OperationNot(string name) : base(name)
        {
            Priority = 9;
            OperandsCount = 1;
        }
        public override Operand<bool> Execute(params Operand<bool>[] operands)
        {
            Operand<bool> result = new Constant<bool>("_temp", false);
            result.Value = !operands[0].Value;

            return result;
        }
    }
}
