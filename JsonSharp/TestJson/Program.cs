using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSharp;

namespace TestJson
{
    class Program
    {
        static void Main(string[] args)
        {
            TestObject obj = new TestObject();
            obj.Init();
            obj.SetArray();
            obj.SetObject();

            Json json = Json.Parse(obj);
            Console.WriteLine(json.ToString());

            Json json1 = new Json();
            json1["firstName"] = "Andrey";
            json1["lastName"] = "Karpov";
            json1["array"] = new int[] { 1, 2, 3 };
            json1["object"] = json;
            Console.WriteLine(json1);
            Console.Read();
        }
    }
}
