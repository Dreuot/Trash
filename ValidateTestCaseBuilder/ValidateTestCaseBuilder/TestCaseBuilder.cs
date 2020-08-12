using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValidateTestCaseBuilder.ExpressionEntities;

namespace ValidateTestCaseBuilder
{
    class TestCaseBuilder
    {
        public string Expression { get; set; }
        public List<ExpressionEntity> Entities { get; set; }
        private int offset;
        string[] lexems = new string[] { @"\(", @"\)", @"[А-Яа-яA-Za-z\d\s]+", @"\|", @"\&", @"\!" };
        char[] spaces = new char[] { ' ', '\n', '\r', '\t' };

        public TestCaseBuilder()
        {
            offset = 0;
            Entities = new List<ExpressionEntity>();
        }

        public TestCaseBuilder(string expression): this()
        {
            Expression = expression;
            Parse();
        }

        private void Parse()
        {
            while (IsBound())
            {
                SkipSpaces();
                for (int i = 0; i < lexems.Length; i++)
                {
                    //string expr = Expression.Substring(offset);
                    Regex r = new Regex(lexems[i]);
                    //var match = r.Match(expr);
                    var match = r.Match(Expression, offset);
                    if (!match.Success || match.Index != offset)
                        continue;

                    offset += match.Length;

                    ExpressionEntity e = EntityFabric.GetEntity(match.Value);
                    Entities.Add(e);
                    break;
                }
            }
        }

        private bool IsBound()
        {
            return offset < Expression.Length;
        }

        private void SkipSpaces()
        {
            while (IsBound() && this.spaces.Any(c => Expression[offset] == c))
                offset++;
        }
    }
}
