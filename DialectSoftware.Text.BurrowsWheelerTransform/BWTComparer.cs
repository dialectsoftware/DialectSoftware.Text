using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

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
