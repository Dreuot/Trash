﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluatorLib.Lexems
{
    public class Lexem
    {
        public string Name { get; private set; }
        public Lexem(string name)
        {
            Name = name;
        }
    }
}
