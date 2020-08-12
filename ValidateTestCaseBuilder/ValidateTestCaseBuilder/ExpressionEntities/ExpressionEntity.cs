using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateTestCaseBuilder.ExpressionEntities
{
    class ExpressionEntity
    {
        public string Name { get; set; }

        public ExpressionEntity(string name)
        {
            Name = name;
        }
    }
}
