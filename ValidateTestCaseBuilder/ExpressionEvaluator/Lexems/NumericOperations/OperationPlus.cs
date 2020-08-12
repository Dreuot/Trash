using System;
using System.Collections.Generic;
using System.Text;
using ExpressionEvaluatorLib.Lexems;

namespace ExpressionEvaluatorLib.Lexems.NumericOperations
{
    public class OperationPlus : Operation<double, double>
    {
        public OperationPlus(string name) : base(name)
        {
            Priority = 10;
            OperandsCount = 2;
        }
        public override Operand<double> Execute(params Operand<double>[] operands)
        {
            Operand<double> result = new Constant<double>("_temp", 0);
            for (int i = 0; i < OperandsCount; i++)
            {
                result.Value += operands[i].Value;
            }

            return result;
        }
    }
}
