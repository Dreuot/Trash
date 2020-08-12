using ExpressionEvaluatorLib.Lexems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ExpressionEvaluatorLib
{
    public class ExpressionEvaluator<T>
    {
        private string expression;
        public List<Lexem> Lexems { get; private set; }
        private List<Lexem> OutArray { get; set; }
        public string[] Variables => OutArray.Where(l => l is Variable<T>).Select(l => l.Name).Distinct().ToArray();
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
                foreach (var lexem in LexemsMap)
                {
                    Regex r = new Regex(lexem.Key);
                    var match = r.Match(Expression, Offset);
                    if (!match.Success || match.Index != Offset)
                        continue;

                    Offset += match.Length;

                    Lexem e = lexem.Value(match.Value);
                    Lexems.Add(e);
                    break;
                }
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
                if (item is Operand<T> operand)
                {
                    OutArray.Add(operand);
                }
                else if (item is Operation<T, T> operation)
                {
                    if (operations.Count == 0)
                    {
                        operations.Push(operation);
                    }
                    else
                    {
                        Operation<T, T> head = operations.Peek() as Operation<T, T>;
                        if (head != null)
                        {
                            if (operation.Priority <= head.Priority)
                                OutArray.Add(operations.Pop());
                        }

                        operations.Push(operation);
                    }
                }
                else if (item is Bracket bracket)
                {
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

        public T Calculate()
        {
            Stack<Operand<T>> operands = new Stack<Operand<T>>();
            foreach (var item in OutArray)
            {
                if(item is Operand<T> operand)
                {
                    operands.Push(operand);
                }
                else if(item is Operation<T, T> operation)
                {
                    Operand<T>[] args = new Operand<T>[operation.OperandsCount];
                    for (int i = operation.OperandsCount - 1; i >= 0; i--)
                        args[i] = operands.Pop();

                    operands.Push(operation.Execute(args));
                }
            }

            return operands.Pop().Value;
        }

        public bool SetVariable(string name, T value)
        {
            var lexems = OutArray.Where(l => l.Name == name);
            bool result = false;
            foreach (var item in lexems)
            {
                result = true;
                (item as Variable<T>).Value = value;
            }

            return result;
        }
        #endregion
    }
}
