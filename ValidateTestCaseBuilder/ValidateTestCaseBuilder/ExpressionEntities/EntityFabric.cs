using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateTestCaseBuilder.ExpressionEntities
{
    static class EntityFabric
    {
        private static Dictionary<string, Type> map = new Dictionary<string, Type>()
        {
            { "(", typeof(Bracket)},
            { ")", typeof(Bracket)},
            { "|", typeof(OperationOr<bool>)},
            { "&", typeof(OperationAnd<bool>)},
            { "!", typeof(OperationNot<bool>)},
        };

        public static ExpressionEntity GetEntity(string name)
        {
            Type t;
            if(!map.TryGetValue(name, out t))
                t = typeof(Operand<bool>);
            
            return (ExpressionEntity)t.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { name });
        }
    }
}
