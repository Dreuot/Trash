using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems.NumericOperations
{
    public class OperationDivide : Operation<double, double>
    {
        public OperationDivide(string name) : base(name)
        {
            Priority = 20;
            OperandsCount = 2;
        }
        public override Operand<double> Execute(params Operand<double>[] operands)
        {
            Operand<double> result = new Constant<double>("_temp", 0);
            result.Value = operands[0].Value / operands[1].Value;

            return result;
        }
    }
}
