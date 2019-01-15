using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class Matrix
    {
        protected double[,] inner;
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }

        public Matrix(int rows, int cols)
        {
            inner = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    inner[i, j] = 0;

            Columns = cols;
            Rows = rows;
        }

        public double this[int row, int column]
        {
            get
            {
                return inner[row, column];
            }
            set
            {
                inner[row, column] = value;
            }
        }

        virtual public void Init(params double[] param)
        {
            int l = Rows * Columns;

            if (param.Length != l)
                throw new ArgumentException($"Неверное количество элементов. Введено {param.Length}, должно быть {l}");

            int current = 0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    inner[i, j] = param[current];
                    current++;
                }
            }
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Columns != right.Rows)
                throw new ArgumentException($"Количество столбцов в левой матрице({left.Columns}) не совпадает с количеством строк в правой матрице ({right.Rows})");

            Matrix result = new Matrix(left.Rows, right.Columns);

            for (int outer = 0; outer < left.Rows; outer++)
            {
                for (int inner = 0; inner < right.Columns; inner++)
                {
                    for (int element = 0; element < left.Columns; element++)
                    {
                        result[outer, inner] += left[outer, element] * right[element, inner];
                    }
                }
            }

            return result;
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.Columns != right.Columns || left.Rows != right.Rows)
                throw new ArgumentException($"Размеры матриц не совпадают {left.Rows} x {left.Columns} и {right.Rows} x {right.Columns}");

            Matrix result = new Matrix(left.Rows, right.Columns);

            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Columns; j++)
                {
                    result[i, j] = left[i, j] + right[i, j];
                }
            }

            return result;
        }

        public Matrix Clone()
        {
            Matrix res = new Matrix(Rows, Columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    res[i, j] = this[i, j];
                }
            }

            return res;
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.Columns != right.Columns || left.Rows != right.Rows)
                throw new ArgumentException($"Размеры матриц не совпадают {left.Rows} x {left.Columns} и {right.Rows} x {right.Columns}");

            Matrix result = new Matrix(left.Rows, right.Columns);

            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Columns; j++)
                {
                    result[i, j] = left[i, j] - right[i, j];
                }
            }

            return result;
        }

        public static Matrix operator /(Matrix left, double number)
        {
            Matrix result = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Columns; j++)
                {
                    result[i, j] = left[i, j] / number;
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix left, double number)
        {
            Matrix result = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Columns; j++)
                {
                    result[i, j] = left[i, j] * number;
                }
            }

            return result;
        }

        public static Matrix operator *(double number, Matrix right)
        {
            Matrix result = new Matrix(right.Rows, right.Columns);
            for (int i = 0; i < right.Rows; i++)
            {
                for (int j = 0; j < right.Columns; j++)
                {
                    result[i, j] = right[i, j] * number;
                }
            }

            return result;
        }

        public Matrix Transpose()
        {
            Matrix result = new Matrix(Columns, Rows);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[j, i] = this[i, j];
                }
            }

            return result;
        }

        public Matrix Gauss(Matrix vector)
        {
            if (vector.Rows != this.Rows && vector.Columns != 1)
                throw new ArgumentException($"Размер вектора свободных членов должен быть {Rows} x 1");

            Matrix result = new Matrix(Rows, 1);
            Matrix transform = this.Clone();
            for (int row = 0; row < Rows - 1; row++)
            {
                double elem = transform[row, row];
                if (elem == 0)
                {
                    int first = row;
                    int second = GetFirstNonZero(transform, row, row);
                    SwapLines(transform, first, second, vector);
                    elem = transform[row, row];
                }

                for (int i = row + 1; i < Rows; i++)
                {
                    double factor = -transform[i, row] / elem;
                    for (int j = row; j < Columns; j++)
                    {
                        transform[i, j] = transform[row, j] * factor + transform[i, j];
                    }

                    vector[i, 0] = vector[row, 0] * factor + vector[i, 0];
                }
            }

            for (int row = 0; row < Rows; row++)
            {
                double leftPart = 0;
                for (int i = 0; i < row; i++)
                {
                    leftPart += transform[Rows - row - 1, Columns - i - 1] * result[Rows - i - 1, 0];
                }

                result[Rows - row - 1, 0] = (vector[Rows - row - 1, 0] - leftPart) / transform[Rows - row - 1, Rows - row - 1];
            }

            return result;
        }

        private void SwapLines(Matrix transform, int first, int second, Matrix vector)
        {
            double[] temp = new double[Columns];
            double vec = vector[first, 0];
            for (int i = 0; i < Columns; i++)
                temp[i] = transform[first, i];

            for (int i = 0; i < Columns; i++)
                transform[first, i] = transform[second, i];
            vector[first, 0] = vector[second, 0];

            for (int i = 0; i < Columns; i++)
                transform[second, i] = temp[i];
            vector[second, 0] = vec;
        }

        private int GetFirstNonZero(Matrix transform, int row, int col)
        {
            int result = row;
            for (int i = row; i < Rows; i++)
                if (transform[i, col] != 0)
                    result = i;

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append(this[i, j]);
                    sb.Append("\t");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        public double[] ToArray()
        {
            double[] result = new double[Rows * Columns];
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    result[Columns * x + y] = this[x, y];
                }
            }

            return result;
        }
    }
}
