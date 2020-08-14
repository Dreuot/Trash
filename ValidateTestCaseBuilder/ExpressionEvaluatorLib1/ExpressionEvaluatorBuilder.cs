using ExpressionEvaluatorLib.Lexems;
using System;
using System.Globalization;

namespace ExpressionEvaluatorLib
{
    public class ExpressionEvaluatorBuilder
    {
        private ExpressionEvaluator ev;
        public ExpressionEvaluatorBuilder()
        {
            ev = new ExpressionEvaluator();
        }

        public static ExpressionEvaluator CreateDefault(string expression)
        {
            ExpressionEvaluatorBuilder builder = new ExpressionEvaluatorBuilder();
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

            builder.AddLexem(@"true", n => new Constant(n, bool.Parse(n)));
            builder.AddLexem(@"false", n => new Constant(n, bool.Parse(n)));
            builder.AddLexem(@"\d+\.*\d*", n => new Constant(n, double.Parse(n, CultureInfo.InvariantCulture)));
            builder.AddLexem(@"[A-Za-zА-Яа-я]+[A-Za-zА-Яа-я\d]*", n => new Variable(n));
            builder.AddLexem("\"[^\"]*\"", n => new Constant(n, n.Substring(1, n.Length - 2)));

            builder.AddOperation(@"\^", (args) => Math.Pow(args[0], args[1]), 30, 2);
            builder.AddOperation(@"\*", (args) => args[0] * args[1], 20, 2);
            builder.AddOperation(@"\/", (args) => args[0] / args[1], 20, 2);
            builder.AddOperation(@"\+", (args) => args[0] + args[1], 10, 2);
            builder.AddOperation(@"\-", (args) => args[0] - args[1], 10, 2);
            builder.AddOperation(@">",  (args) => args[0] > args[1], 9, 2);
            builder.AddOperation(@">=", (args) => args[0] >= args[1], 9, 2);
            builder.AddOperation(@"<",  (args) => args[0] < args[1], 9, 2);
            builder.AddOperation(@"<=", (args) => args[0] <= args[1], 9, 2);
            builder.AddOperation(@"=", (args) => args[0] == args[1], 8, 2);
            builder.AddOperation(@"\!", (args) => !args[0], 7, 1);
            builder.AddOperation(@"\&", (args) => args[0] && args[1], 6, 2);
            builder.AddOperation(@"\|", (args) => args[0] || args[1], 5, 2);

            return builder.Build();
        }

        public ExpressionEvaluatorBuilder SetExpression(string expression)
        {
            ev.Expression = expression;
            return this;
        }

        public ExpressionEvaluatorBuilder AddLexem(string pattern, Func<string, Lexem> creator)
        {
            ev.LexemsMap.Add(pattern, creator);
            return this;
        }

        public ExpressionEvaluatorBuilder AddOperation(string pattern, Func<dynamic[], dynamic> func, int priority, int operandsCount)
        {
            ev.LexemsMap.Add(pattern, n => new Operation(pattern, func) { Priority = priority, OperandsCount = operandsCount});
            return this;
        }

        public ExpressionEvaluatorBuilder AddSkip(char symbol)
        {
            ev.Skipable.Add(symbol);
            return this;
        }

        public ExpressionEvaluator Build()
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
