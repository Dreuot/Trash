using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidateTestCaseBuilder.ExpressionEntities;

namespace ValidateTestCaseBuilder
{
    class PolandNotation
    {
        private IEnumerable<ExpressionEntity> entities;
        public IEnumerable<ExpressionEntity> Entities
        {
            get
            {
                return entities;
            }
            set
            {
                entities = value;
                FormArray();
            }
        }
        public List<ExpressionEntity> OutArray { get; set; }
        private Stack<ExpressionEntity> Operations { get; set; }

        public PolandNotation(IEnumerable<ExpressionEntity> entities)
        {
            OutArray = new List<ExpressionEntity>();
            Operations = new Stack<ExpressionEntity>();
            Entities = entities;
        }

        public bool Calculate()
        {
            Stack<Operand<bool>> operands = new Stack<Operand<bool>>();
            foreach(var item in OutArray)
            {
                if(item is Operand<bool>)
                {
                    operands.Push(item as Operand<bool>);
                }
                else
                {
                    Operation<bool> oper = item as Operation<bool>;
                    Operand<bool>[] ops = new Operand<bool>[oper.OperandsCount];
                    for (int i = 0; i < oper.OperandsCount; i++)
                        ops[i] = operands.Pop();

                    operands.Push(oper.Execute(ops));
                }
            }

            return operands.Pop().Value;
        }

        private void FormArray()
        {
            foreach (var item in Entities)
            {
                if (item is Operand<bool>)
                {
                    OutArray.Add(item);
                }
                else if (item is Operation<bool>)
                {
                    var oper = item as Operation<bool>;
                    if (Operations.Count == 0)
                    {
                        Operations.Push(oper);
                    }
                    else
                    {
                        ExpressionEntity head = Operations.Peek();
                        if (head is Operation<bool>)
                        {
                            var headOper = head as Operation<bool>;
                            if (oper.Priority <= headOper.Priority)
                            {
                                OutArray.Add(Operations.Pop());
                                Operations.Push(oper);
                            }
                            else
                            {
                                Operations.Push(oper);
                            }
                        }
                        else
                        {
                            Operations.Push(oper);
                        }
                    }
                }
                else if (item is Bracket)
                {
                    if (item.Name == "(")
                    {
                        Operations.Push(item);
                    }
                    else
                    {
                        var head = Operations.Pop();
                        while (head.Name != "(")
                        {
                            OutArray.Add(head);
                            head = Operations.Pop();
                        }
                    }
                }
            }

            while (Operations.Count != 0)
            {
                OutArray.Add(Operations.Pop());
            }
        }
    }
}
