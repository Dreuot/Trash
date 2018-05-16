using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sampler
{
    public class Sampler
    {
        public string Separatop { get; set; }
        public SamplingData Data { get; set; }
        public int RowCount => Data.RowCount;
        public IEnumerable<string> Captions => Data.Captions;
        public Func<string, string> ClassSelection { get; set; }
        public string Info { get; private set; }

        public Sampler(string separator, Func<string, string> classSelection = null)
        {
            Separatop = separator;
            Data = new SamplingData();
            ClassSelection = classSelection != null ? classSelection : (x) => x;
        }

        public string this[int row, string key]
        {
            get
            {
                return Data[row, key];
            }
        }

        public void LoadData(string fileName, bool withCaptions = false)
        {
            string[] data = File.ReadAllLines(fileName);
            List<string> captions = new List<string>();
            string[] firstString = data[0].Split(new string[] { Separatop }, StringSplitOptions.None);
            if (withCaptions)
                foreach (string caption in firstString)
                    captions.Add(caption);
            else
                for (int i = 0; i < firstString.Length; i++)
                    captions.Add(i.ToString());

            for (int i = 0 + ((withCaptions) ? 1 : 0); i < data.Length; i++)
            {
                string[] current = data[i].Split(new string[] { Separatop }, StringSplitOptions.None);
                Dictionary<string, string> obj = new Dictionary<string, string>();
                int len = current.Length;
                for (int j = 0; j < captions.Count; j++)
                    obj.Add(captions[j], (j < len) ? current[j] : "");

                Data.AddRow(obj);
            }
        }

        public SamplingData MakeSelect(double percent, string key, int layer)
        {
            List<double> values = new List<double>();
            for (int i = 0; i < Data.RowCount; i++)
            {
                values.Add(double.Parse(Data[i][key]));
            }

            double[] sorted = values.ToArray();
            Array.Sort(sorted);
            int rows = Data.RowCount;
            int step = rows / layer;

            Func<string, string> selector = (value) =>
            {
                double asNum = double.Parse(value);
                string result = "0";
                int l = 1;
                bool found = false;
                while (!found)
                {
                    double current = 0;
                    if (l * step < rows)
                    {
                        current = sorted[l * step];
                    }
                    else
                    {
                        current = sorted[rows - 1];
                        l--;
                    }

                    if (asNum <= current)
                    {
                        found = true;
                        result = l.ToString();
                    }

                    l++;
                }

                return result;
            };

            return MakeSelect(percent, key, selector);
        }

        private SamplingData MakeSelect(double percent, string key, Func<string, string> selector)
        {
            Info = "";
            SamplingData data = new SamplingData();
            Dictionary<string, List<Dictionary<string, string>>> classes = new Dictionary<string, List<Dictionary<string, string>>>();
            for (int i = 0; i < Data.RowCount; i++)
            {
                string @class = selector(Data[i, key]);
                if (classes.ContainsKey(@class))
                {
                    classes[@class].Add(Data[i]);
                }
                else
                {
                    classes.Add(@class, new List<Dictionary<string, string>>());
                    classes[@class].Add(Data[i]);
                }
            }

            Info += $"Всего строк {RowCount}, выбираем {RowCount * percent}\n";

            var groupsSelect = classes.Select(n =>
            {
                return new { Name = n.Key, Count = (int)Math.Round(n.Value.Count * percent) };
            });

            Random r = new Random();
            foreach (var groupSelect in groupsSelect)
            {
                Info += $"Группа {groupSelect.Name} количество {groupSelect.Count}\n";
                Dictionary<string, string> obj = new Dictionary<string, string>();
                int classCount = classes[groupSelect.Name].Count;
                List<int> indexes = new List<int>(classCount);
                for (int i = 0; i < classCount; i++)
                    indexes.Add(i);

                for (int i = 0; i < groupSelect.Count; i++)
                {
                    int randInd = indexes[r.Next(0, indexes.Count)];
                    indexes.Remove(randInd);
                    data.AddRow(classes[groupSelect.Name][randInd]);
                }
            }

            return data;
        }

        public SamplingData MakeSelect(double percent, string key)
        {
            return MakeSelect(percent, key, ClassSelection);
        }
    }
}
