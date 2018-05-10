using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Graph
    {
        public static readonly int Infinity = int.MaxValue;
        private GraphNode start;

        public int Size { get; private set; }
        public int[,] Matrix { get; private set; }
        public GraphNode[] Nodes { get; private set; }
        public GraphNode Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
                start.FinalResult = true;
                Reset();
            }
        }

        public GraphNode End { get; private set; }

        public Graph(int size)
        {
            Size = size;
            Matrix = new int[size, size];
            Nodes = new GraphNode[size];
        }

        public void Initialize(int[] distances)
        {
            if (distances.Length != Size * Size)
                throw new ArgumentException($"Количество элементов должно быть равно {Size * Size}, Вы ввели {distances.Length}");

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Matrix[i, j] = distances[Size * i + j];

            for (int i = 0; i < Size; i++)
                Nodes[i] = new GraphNode(i, 0);

            Start = Nodes[0];
        }

        private void Reset()
        {
            for (int i = 0; i < Size; i++)
            {
                if (i != Start.Number)
                    Nodes[i].Distance = Infinity;
                else
                    Nodes[i].Distance = 0;
            }
        }

        public void SetStart(int start)
        {
            if (start < 0 || start >= Size)
                throw new ArgumentOutOfRangeException($"Номер узла должен находиться в пределах [0, {Size - 1}");

            Start = Nodes[start];
        }

        public void SetEnd(int start)
        {
            if (start < 0 || start >= Size)
                throw new ArgumentOutOfRangeException($"Номер узла должен находиться в пределах [0, {Size - 1}");

            End = Nodes[start];
        }

        private bool HaveNotFinite()
        {
            return Nodes.Any(n => n.FinalResult == false);
        }

        public void Dijkstra()
        {
            Reset();
            GraphNode p = Start;
            while (HaveNotFinite())
            {
                var nonMarked = Nodes.Where(n => n.FinalResult == false);
                foreach (var item in nonMarked)
                {
                    if(Matrix[p.Number, item.Number] != Infinity)
                    {
                        int old = item.Distance;
                        int @new = p.Distance + Matrix[p.Number, item.Number];
                        item.Distance = old > @new ? @new : old;
                    }
                }

                p = nonMarked.Min();
                p.FinalResult = true;
            }
        }

        public void Dynamic()
        {
            Reset();
            var s = Start;
            for (int i = 1; i < Size; i++)
            {
                var T = Nodes.Where(n => Matrix[n.Number, Nodes[i].Number] != Infinity).ToArray();
                int old = Nodes[i].Distance;
                int count = T.Count();

                if(count > 0)
                {
                    int[] mins = new int[count];
                    for (int j = 0; j < count; j++)
                        mins[j] = T[j].Distance + Matrix[T[j].Number, Nodes[i].Number];

                    int min = mins.Min();
                    Nodes[i].Distance = old > min ? min : old;
                }
            }
        }

        public string Way()
        {
            int way = End.Distance;
            var p = End;
            StringBuilder sb = new StringBuilder();
            List<int> numbers = new List<int>();
            numbers.Add(p.Number);
            while (way > 0)
            {
                foreach (var item in Nodes)
                {
                    if (Matrix[item.Number, p.Number] != Infinity)
                    {
                        if (p.Distance - Matrix[item.Number, p.Number] == item.Distance)
                        {
                            p = item;
                            numbers.Add(p.Number);
                            way = item.Distance;
                        }
                    }
                }
            }

            numbers.Reverse();
            sb.Append("( ");
            foreach (var item in numbers)
                sb.Append($"{item} ");

            sb.Append(")");

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Matrix[i, j] == Infinity)
                        sb.Append("inf\t");
                    else
                        sb.Append($"{Matrix[i, j]}\t");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        public string Distances()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                string distance = Nodes[i].Distance == Infinity ? "inf" : Nodes[i].Distance.ToString();
                sb.Append($"Узел{i}: {distance}\n");
            }

            return sb.ToString();
        }
    }
}
