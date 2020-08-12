using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateTestCaseBuilder.ExpressionEntities
{
    abstract class Operation<T> : ExpressionEntity
    {
        public int Priority { get; protected set; }
        public int OperandsCount { get; protected set; }
        public Operation(string name) : base(name)
        {
        }

        //public abstract Operand<T> Execute(Operand<T> left, Operand<T> right);
        public abstract Operand<T> Execute(params Operand<T>[] operands);
    }
}
