using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLib
{
    public struct YIQ
    {
        public double Y { get; set; }
        public double Ic { get; set; }
        public double Qc { get; set; }

        public YIQ(double y, double ic, double qc)
        {
            Y = y;
            Ic = ic;
            Qc = qc;
        }
    }
}
