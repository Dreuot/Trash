using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateTestCaseBuilder
{
    class AccomodationGenerator
    {
        public int Length { get; private set; }
        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public int[][] Inner { get; private set; }

        public void Calculate()
        {
            int capacity = (int)Math.Pow(Count, Length);
            Inner = new int[capacity][];
            for (int i = 0; i < capacity; i++)
            {
                Inner[i] = new int[Length];
                for (int j = 0; j < Length; j++)
                {
                    int delimeter = capacity / (int)(Math.Pow(Count, j + 1));
                    int value = i / delimeter % Count;
                    Inner[i][j] = value;
                }
            }
        }

        public AccomodationGenerator(int length, int count)
        {
            Length = length;
            Count = count;
            Capacity = (int)Math.Pow(Count, Length);
            Calculate();
        }
    }
}
