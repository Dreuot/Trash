using System;
using System.Collections.Generic;

namespace Sampler
{
    public class SamplingData
    {
        private List<Dictionary<string, string>> Inner { get; set; }
        public int RowCount => Inner.Count;
        public IEnumerable<string> Captions => Inner[0]?.Keys;

        public SamplingData()
        {
            Inner = new List<Dictionary<string, string>>();
        }

        public string this[int row, string key]
        {
            get
            {
                if (row >= RowCount || row < 0)
                    throw new IndexOutOfRangeException("Нет такой строки");

                Dictionary<string, string> obj = Inner[row];
                if (!obj.ContainsKey(key))
                    throw new IndexOutOfRangeException("Данного свойства не существует");
                else
                    return obj[key];
            }
        }

        public Dictionary<string, string> this[int row]
        {
            get
            {
                if (row >= RowCount || row < 0)
                    throw new IndexOutOfRangeException("Нет такой строки");

                return Inner[row];
            }
        }

        public void AddRow(Dictionary<string, string> obj)
        {
            Inner.Add(obj);
        }
    }
}