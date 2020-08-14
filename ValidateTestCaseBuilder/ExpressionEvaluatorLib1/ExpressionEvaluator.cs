using ExpressionEvaluatorLib.Lexems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ExpressionEvaluatorLib
{
    public class ExpressionEvaluator
    {
        private string expression;
        public List<Lexem> Lexems { get; private set; }
        public List<Lexem> OutArray { get; private set; }
        public string[] Variables => OutArray.Where(l => l is Variable).Select(l => l.Name).Distinct().ToArray();
        public int Offset { get; private set; }
        public Dictionary<string, Func<string, Lexem>> LexemsMap { get; private set; }
        public List<char> Skipable { get; private set; }
        public string Expression {
            get
            {
                return expression;
            }
            set
            {
                expression = value;
                Parse();
                FormArray();
            }
        }

        internal ExpressionEvaluator()
        {
            LexemsMap = new Dictionary<string, Func<string, Lexem>>();
            Skipable = new List<char>();
            Lexems = new List<Lexem>();
            Expression = "";
        }

        internal ExpressionEvaluator(string expression, Dictionary<string, Func<string, Lexem>> lexemsMap, char[] skipable)
        {
            LexemsMap = lexemsMap;
            Skipable = skipable.ToList() ?? new List<char>();
            Lexems = new List<Lexem>();
            Expression = expression;
        }

        #region ParseExpression
        public void Parse()
        {
            if (Expression == "" || LexemsMap.Count == 0)
                return;

            Lexems = new List<Lexem>();
            Offset = 0;
            while (IsBound())
            {
                SkipSpaces();
                bool notFound = true;
                foreach (var lexem in LexemsMap)
                {
                    Regex r = new Regex(lexem.Key);
                    var match = r.Match(Expression, Offset);
                    if (!match.Success || match.Index != Offset)
                        continue;

                    Offset += match.Length;

                    Lexem e = lexem.Value(match.Value);
                    Lexems.Add(e);
                    notFound = false;
                    break;
                }

                if (notFound)
                    break;
            }

            FormArray();
        }

        private bool IsBound()
        {
            return Offset < Expression.Length;
        }

        private void SkipSpaces()
        {
            while (IsBound() && Skipable.Any(c => Expression[Offset] == c))
                Offset++;
        }
        #endregion

        #region EvaluateExpression
        private void FormArray()
        {
            OutArray = new List<Lexem>();
            Stack<Lexem> operations = new Stack<Lexem>();
            foreach (var item in Lexems)
            {
                if (item.Type == LexemType.Operand)
                {
                    var operand = item as Operand;
                    OutArray.Add(operand);
                }
                else if (item.Type == LexemType.Operation)
                {
                    var operation = item as Operation;
                    if (operations.Count == 0)
                    {
                        operations.Push(operation);
                    }
                    else
                    {
                        do
                        {
                            Operation head = operations.Peek() as Operation;
                            if (head == null)
                                break;

                            if (operation.Priority <= head.Priority)
                            {
                                OutArray.Add(operations.Pop());
                            }
                            else
                            {
                                break;
                            }
                        } while (operations.Count != 0);

                        operations.Push(operation);
                    }
                }
                else if (item.Type == LexemType.Bracket)
                {
                    var bracket = item as Bracket;
                    if (bracket.IsLeft)
                    {
                        operations.Push(bracket);
                    }
                    else
                    {
                        var head = operations.Pop();
                        while (head.Name != bracket.PairBracket)
                        {
                            OutArray.Add(head);
                            head = operations.Pop();
                        }
                    }
                }
            }

            while (operations.Count != 0)
            {
                OutArray.Add(operations.Pop());
            }
        }

        public dynamic Calculate()
        {
            Stack<Operand> operands = new Stack<Operand>();
            foreach (var item in OutArray)
            {
                if(item is Operand operand)
                {
                    operands.Push(operand);
                }
                else if(item is Operation operation)
                {
                    dynamic[] args = new dynamic[operation.OperandsCount];
                    for (int i = operation.OperandsCount - 1; i >= 0; i--)
                        args[i] = operands.Pop().Value;

                    var result = new Operand("_temp");
                    result.Value = operation.Execute(args);
                    operands.Push(result);
                }
            }

            return operands.Pop().Value;
        }

        public bool SetVariable(string name, dynamic value)
        {
            var lexems = OutArray.Where(l => l.Name == name);
            bool result = false;
            foreach (var item in lexems)
            {
                result = true;
                (item as Operand).Value = value;
            }

            return result;
        }
        #endregion
    }
}
