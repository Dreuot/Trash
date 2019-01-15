using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonSharp
{
    class Json
    {
        private object primitiveValue;
        private Dictionary<string, object> data;
        public JsonType Type { get; }

        public Json()
        {
            Type = JsonType.Object;
            data = new Dictionary<string, object>();
        }

        private Json(JsonType type)
        {
            primitiveValue = null;
            Type = type;
            data = new Dictionary<string, object>();
        }

        public object this[string field]
        {
            get
            {
                data.TryGetValue(field, out object result);
                return result;
            }
            set
            {
                if (!(value is Json))
                {
                    Json asJson = Json.Parse(value);
                    data[field] = asJson;
                }
                else
                    data[field] = value;
            }
        }

        public static Json Parse(string jsonString)
        {
            return null;
        }

        public object GetValue()
        {
            if (Type == JsonType.Primitive)
                return this.primitiveValue;
            else
                throw new Exception("Json type must be is 'Primitive Type'");
        }

        public object TryGetValue(out object value)
        {
            if (Type == JsonType.Primitive)
            {
                value = this.primitiveValue;
                return true;
            }

            value = null;
            return false;
        }

        public static Json Parse(object obj)
        {
            if (obj == null)
                return new Json(JsonType.Primitive);

            Type type = obj.GetType();
            JsonType jsType = GetJsonType(obj);
            switch (jsType)
            {
                case JsonType.Primitive:
                    return PrimitiveToJson(obj);
                case JsonType.Object:
                    return ObjectToJson(obj);
                case JsonType.Array:
                    return ArrayToJson(obj as IEnumerable);
                default:
                    return null;
            }
        }

        private static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            switch (this.Type)
            {
                case JsonType.Primitive:
                    object val = GetValue();
                    string str;
                    if (val == null)
                        str = "null";
                    else if (val.GetType().Name == typeof(string).Name)
                        str = "\"" + val.ToString() + "\"";
                    else if (IsNumber(val))
                        str = val.ToString().Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
                    else if (val is bool)
                        str = val.ToString().ToLower();
                    else
                        str = val.ToString();

                    return str;
                case JsonType.Object:
                    sb.Append("{");
                    foreach (var item in data)
                    {
                        sb.Append("\"");
                        sb.Append(item.Key);
                        sb.Append("\"");
                        sb.Append(":");
                        sb.Append(item.Value.ToString());
                        sb.Append(",");
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("}");

                    return sb.ToString();
                case JsonType.Array:
                    sb.Append("[");
                    foreach (var item in data)
                    {
                        sb.Append(item.Value.ToString());
                        sb.Append(",");
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("]");

                    return sb.ToString();
                default:
                    return "";
            }
        }

        private static Json PrimitiveToJson(object obj)
        {
            Json json = new Json(JsonType.Primitive);
            json.primitiveValue = obj;
            return json;
        }

        private static Json ObjectToJson(object obj)
        {
            Type type = obj.GetType();
            Json json = new Json(JsonType.Object);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                var propValue = prop.GetValue(obj);
                json[prop.Name] = propValue;
            }

            return json;
        }

        private static Json ArrayToJson(IEnumerable obj)
        {
            Json json = new Json(JsonType.Array);
            int i = 0;
            foreach (var item in obj)
            {
                json[i.ToString()] = item;
                i++;
            }

            return json;
        }


        private static JsonType GetJsonType(object obj)
        {
            Type type = obj.GetType();
            if (type.IsPrimitive || type.Name == typeof(decimal).Name || type.Name == typeof(string).Name)
            {
                return JsonType.Primitive;
            }
            else if (type.IsValueType)
            {
                return JsonType.Object;
            }
            else if (obj as IEnumerable != null)
            {
                return JsonType.Array;
            }
            else
            {
                return JsonType.Object;
            }
        }

        //private static Type GetElementTypeOfEnumerable(object o)
        //{
        //    if (!(o is IEnumerable enumerable))
        //        return null;

        //    Type[] interfaces = enumerable.GetType().GetInterfaces();
        //    Type elementType = (from i in interfaces
        //                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
        //                        select i.GetGenericArguments()[0]).FirstOrDefault();
        //    if (elementType == null || elementType == typeof(object))
        //    {
        //        object firstElement = enumerable.Cast<object>().FirstOrDefault();
        //        if (firstElement != null)
        //            elementType = firstElement.GetType();
        //    }
        //    return elementType;
        //}
    }
}
