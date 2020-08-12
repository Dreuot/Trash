using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateTestCaseBuilder.ExpressionEntities
{
    class OperationNot<T> : Operation<T>
    {
        public OperationNot(string name) : base(name)
        {
            Priority = 20;
            OperandsCount = 1;
        }

        public override Operand<T> Execute(params Operand<T>[] operands)
        {
            Operand<T> result = new Operand<T>("_temp" + Operand<T>.Counter.ToString());
            bool l = (bool)(object)operands[0].Value;
            result.Value = (T)(object)(!l);

            return result;
        }
    }
}
