using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch
{
    class PointValue : IComparable<PointValue>
    {
        public Point Point { get; set; }
        public int X
        {
            get
            {
                return Point.X;
            }
            set
            {
                Point point = new Point
                {
                    Y = Point.Y,
                    X = value
                };
                Point = point;
            }
        }
        public int Y
        {
            get
            {
                return Point.Y;
            }
            set
            {
                Point point = new Point
                {
                    X = Point.X,
                    Y = value
                };
                Point = point;
            }
        }
        public double Value { get; set; }

        public PointValue()
        {
            Point = new Point();
            Value = 0;
        }

        public PointValue(Point point, double value)
        {
            Point = point;
            Value = value;
        }

        public int CompareTo(PointValue obj)
        {
            return Value.CompareTo(obj.Value);
        }
    }
}
