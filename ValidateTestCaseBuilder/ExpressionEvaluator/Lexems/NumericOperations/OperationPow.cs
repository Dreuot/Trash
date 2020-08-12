using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems.NumericOperations
{
    public class OperationPow : Operation<double, double>
    {
        public OperationPow(string name) : base(name)
        {
            Priority = 30;
            OperandsCount = 2;
        }

        public override Operand<double> Execute(params Operand<double>[] operands)
        {
            Operand<double> result = new Constant<double>("_temp", 0);
            result.Value = Math.Pow(operands[0].Value, operands[1].Value);

            return result;
        }
    }
}
