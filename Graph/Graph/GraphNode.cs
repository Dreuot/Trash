using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class GraphNode: IComparable<GraphNode>
    {
        public int Number { get; set; }
        public int Distance { get; set; }
        public bool FinalResult { get; set; }

        public GraphNode(int number, int distance)
        {
            Number = number;
            Distance = distance;
            FinalResult = false;
        }

        public int CompareTo(GraphNode other)
        {
            return Distance.CompareTo(other.Distance);
        }
    }
}
