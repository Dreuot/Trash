using ExpressionEvaluatorLib.Lexems;
using ExpressionEvaluatorLib.Lexems.BooleanOperations;
using ExpressionEvaluatorLib.Lexems.CompareOperations;
using ExpressionEvaluatorLib.Lexems.NumericOperations;
using System;
using System.Globalization;

namespace ExpressionEvaluatorLib
{
    public class ExpressionEvaluatorBuilder<T>
    {
        private ExpressionEvaluator<T> ev;
        public ExpressionEvaluatorBuilder()
        {
            ev = new ExpressionEvaluator<T>();
        }

        public static ExpressionEvaluator<double> CreateNumeric(string expression)
        {
            ExpressionEvaluatorBuilder<double> builder = new ExpressionEvaluatorBuilder<double>();
            builder.SetExpression(expression);

            builder.AddSkip('\n');
            builder.AddSkip('\r');
            builder.AddSkip('\t');
            builder.AddSkip(' ');

            builder.AddLexem(@"\(", n => new Bracket(n, true, ")"));
            builder.AddLexem(@"\)", n => new Bracket(n, false, "("));
            builder.AddLexem(@"\[", n => new Bracket(n, true, "]"));
            builder.AddLexem(@"\]", n => new Bracket(n, false, "["));
            builder.AddLexem(@"\{", n => new Bracket(n, true, "}"));
            builder.AddLexem(@"\}", n => new Bracket(n, false, "{"));

            builder.AddLexem(@"[A-Za-zА-Яа-я]+[A-Za-zА-Яа-я\d]*", n => new Variable<double>(n));
            builder.AddLexem(@"\d+\.*\d*", n => new Constant<double>(n, double.Parse(n, CultureInfo.InvariantCulture)));

            builder.AddLexem(@"\+", n => new OperationPlus(n));
            builder.AddLexem(@"\-", n => new OperationMinus(n));
            builder.AddLexem(@"\*", n => new OperationMultiply(n));
            builder.AddLexem(@"\/", n => new OperationDivide(n));
            builder.AddLexem(@"\^", n => new OperationPow(n));

            return builder.Build();
        }

        public static ExpressionEvaluator<bool> CreateBool(string expression)
        {
            ExpressionEvaluatorBuilder<bool> builder = new ExpressionEvaluatorBuilder<bool>();
            builder.SetExpression(expression);

            builder.AddSkip('\n');
            builder.AddSkip('\r');
            builder.AddSkip('\t');
            builder.AddSkip(' ');

            builder.AddLexem(@"\(", n => new Bracket(n, true, ")"));
            builder.AddLexem(@"\)", n => new Bracket(n, false, "("));
            builder.AddLexem(@"\[", n => new Bracket(n, true, "]"));
            builder.AddLexem(@"\]", n => new Bracket(n, false, "["));
            builder.AddLexem(@"\{", n => new Bracket(n, true, "}"));
            builder.AddLexem(@"\}", n => new Bracket(n, false, "{"));

            builder.AddLexem(@"true", n => new Constant<bool>(n, bool.Parse(n)));
            builder.AddLexem(@"false", n => new Constant<bool>(n, bool.Parse(n)));
            builder.AddLexem(@"[A-Za-zА-Яа-я]+[A-Za-zА-Яа-я\d]*", n => new Variable<bool>(n));

            builder.AddLexem(@"\&", n => new OperationAnd(n));
            builder.AddLexem(@"\|", n => new OperationOr(n));
            builder.AddLexem(@"\!", n => new OperationNot(n));

            return builder.Build();
        }

        public static ExpressionEvaluator<bool> CreateLogical(string expression)
        {
            ExpressionEvaluatorBuilder<bool> builder = new ExpressionEvaluatorBuilder<bool>();
            builder.SetExpression(expression);

            builder.AddSkip('\n');
            builder.AddSkip('\r');
            builder.AddSkip('\t');
            builder.AddSkip(' ');

            builder.AddLexem(@"\(", n => new Bracket(n, true, ")"));
            builder.AddLexem(@"\)", n => new Bracket(n, false, "("));
            builder.AddLexem(@"\[", n => new Bracket(n, true, "]"));
            builder.AddLexem(@"\]", n => new Bracket(n, false, "["));
            builder.AddLexem(@"\{", n => new Bracket(n, true, "}"));
            builder.AddLexem(@"\}", n => new Bracket(n, false, "{"));

            builder.AddLexem(@"true", n => new Constant<bool>(n, bool.Parse(n)));
            builder.AddLexem(@"false", n => new Constant<bool>(n, bool.Parse(n)));
            builder.AddLexem(@"\d+\.*\d*", n => new Constant<double>(n, double.Parse(n, CultureInfo.InvariantCulture)));

            builder.AddLexem(@"\+", n => new OperationPlus(n));
            builder.AddLexem(@"\-", n => new OperationMinus(n));
            builder.AddLexem(@"\*", n => new OperationMultiply(n));
            builder.AddLexem(@"\/", n => new OperationDivide(n));
            builder.AddLexem(@"\^", n => new OperationPow(n));
            builder.AddLexem(@"\&", n => new OperationAnd(n));
            builder.AddLexem(@"\|", n => new OperationOr(n));
            builder.AddLexem(@"\!", n => new OperationNot(n));
            builder.AddLexem(@">", n => new OperationGreater(n));

            return builder.Build();
        }

        public ExpressionEvaluatorBuilder<T> SetExpression(string expression)
        {
            ev.Expression = expression;
            return this;
        }

        public ExpressionEvaluatorBuilder<T> AddLexem(string pattern, Func<string, Lexem> creator)
        {
            ev.LexemsMap.Add(pattern, creator);
            return this;
        }

        public ExpressionEvaluatorBuilder<T> AddSkip(char symbol)
        {
            ev.Skipable.Add(symbol);
            return this;
        }

        public ExpressionEvaluator<T> Build()
        {
            if (ev.Expression == "")
                throw new Exception("Выражение не может быть пустым");
            if (ev.LexemsMap.Count == 0)
                throw new Exception("Не описаны правила формирования лексем");

            ev.Parse();
            return ev;
        }
    }
}
