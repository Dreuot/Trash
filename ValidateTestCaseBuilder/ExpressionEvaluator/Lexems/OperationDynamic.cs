using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    class OperationDynamic : Lexem
    {
        public int Priority { get; set; }
        public int OperandsCount { get; set; }
        public Func<dynamic[], dynamic> Function { get; set; }
        public OperationDynamic(string name, Func<dynamic[], dynamic> func) : base(name)
        {
            Type = LexemType.Operation;
            Function = func;
        }

        public dynamic Execute(params dynamic[] operands)
        {
            return Function(operands);
        }
    }
}
