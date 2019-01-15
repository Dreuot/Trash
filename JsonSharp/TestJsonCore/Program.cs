using System;

namespace TestJsonCore
{
    class Program
    {
        static void Main(string[] args)
        {
            TestObject obj = new TestObject();
            obj.Init();
            obj.SetArray();
            obj.SetObject();

            Json json = new Json();
        }
    }
}
