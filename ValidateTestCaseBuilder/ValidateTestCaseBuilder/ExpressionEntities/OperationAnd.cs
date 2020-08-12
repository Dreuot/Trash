using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateTestCaseBuilder.ExpressionEntities
{
    class OperationAnd<T> : Operation<T> 
    {
        public OperationAnd(string name): base(name)
        {
            Priority = 10;
            OperandsCount = 2;
        }

        public override Operand<T> Execute(params Operand<T>[] operands)
        {
            Operand<T> result = new Operand<T>("_temp" + Operand<T>.Counter.ToString());
            bool l = (bool)(object)operands[1].Value;
            bool r = (bool)(object)operands[0].Value;
            result.Value = (T)(object)(l && r);

            return result;
        }
    }
}
