using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    public class LineSegment
    {
        public ulong Start { get; set; }
        public ulong End { get; set; }


        public LineSegment()
        {
            Start = 0;
            End = 0;
        }

        public LineSegment(ulong start, ulong end)
        {
            Start = start;
            End = end;
        }
    }
}
