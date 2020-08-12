using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public class Bracket : Lexem
    {
        public bool IsLeft { get; private set; }
        public string PairBracket { get; private set; }
        public Bracket(string name, bool isLeft, string pairBracket) : base(name)
        {
            IsLeft = isLeft;
            PairBracket = pairBracket;
        }
    }
}
