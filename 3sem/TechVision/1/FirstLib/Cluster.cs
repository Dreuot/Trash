using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLib
{
    struct Cluster
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Num { get; set; }

        public Cluster(int x, int y, int num)
        {
            X = x;
            Y = y;
            Num = num;
        }
    }
}
