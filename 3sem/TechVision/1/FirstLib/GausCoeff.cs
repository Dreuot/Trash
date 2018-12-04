using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstLib
{
    class GausCoeff
    {
        public int N { get; set; }
        public double Sigma { get; set; }
        public double B { get; set; }
        double[] b { get; set; }

        public GausCoeff(double sigma)
        {
            double q1 = 0;
            if(sigma >= 2.5)
            {
                q1 = 0.98711 * sigma - 0.96330;
            }
            else if (sigma >=0.5 && sigma < 2.5)
            {
                q1 = 3.97156 - (4.14554 * Math.Sqrt(1 - (0.26891 * sigma)));
            }
            else
            {
                q1 = 0.1147705018520355224609375;
            }

            double q2 = q1 * q1;
            double q3 = q1 * q2;

            b = new double[4];
            b[0] = 1.57825 + (2.44413 * q1) + (1.4281 * q2) + (0.422205 * q3);
            b[1] = (2.44413 * q1) + (2.85618 * q2) + (1.26661 * q3);
            b[2] = -((1.4281 * q2) + (1.26661 * q3));
            b[3] = 0.422205 * q3;

            N = 3;
            Sigma = sigma;
            B = 1 - ((b[1] + b[2] + b[3]) / b[0]);
        }
    }
}
