using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtfsLib
{
    /// <summary>
    /// Отрезок нерезидентного аттрибута, хранит адреса начального и конечного кластеров
    /// </summary>
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
