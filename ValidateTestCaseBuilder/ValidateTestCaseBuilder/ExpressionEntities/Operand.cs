using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateTestCaseBuilder.ExpressionEntities
{
    class Operand<T> : ExpressionEntity
    {
        public static int counter = 0;
        public static int Counter => counter++;
        public T Value { get; set; }

        public Operand(string name) : base(name)
        {
            Value = default(T);
        }

        public Operand(string name, T value) : this(name)
        {
            Value = value;
        }
    }
}
