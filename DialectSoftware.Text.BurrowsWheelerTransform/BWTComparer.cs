using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialectSoftware.Text
{
    internal class BWTComparer : IComparer<int[]>
    {
        public int Compare(int[] x, int[] y)
        {
            var X = x[0].CompareTo(y[0]) + x[1].CompareTo(y[1]);
            var Y = y[0].CompareTo(x[0]) + y[1].CompareTo(x[1]);
            return X - Y;
        }
    }
}
