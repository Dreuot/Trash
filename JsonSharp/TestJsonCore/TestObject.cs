using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJsonCore
{
    class TestObject
    {
        public int Integer { get; set; }
        public decimal Decimal { get; set; }
        public bool Boolean { get; set; }
        public string String { get; set; }
        public object[] Array { get; set; }
        public TestObject Object { get; set; }

        public TestObject()
        {
        }

        public void Init()
        {
            Integer = 1;
            Decimal = 12.3m;
            Boolean = true;
            String = "Test";
        }

        public void SetArray()
        {
            Array = new object[3];
            Array[0] = 1;
            Array[1] = "2";
            TestObject test = new TestObject();
            test.Init();
            Array[2] = test;
        }

        public void SetObject()
        {
            TestObject test = new TestObject();
            test.Init();
            Object = test;
        }
    }
}
