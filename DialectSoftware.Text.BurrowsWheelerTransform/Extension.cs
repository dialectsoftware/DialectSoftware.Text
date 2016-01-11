using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Text
{
    internal static class Extension
    {
        public static IOrderedEnumerable<int[]> ApplyBurrowsWheelerTransform(this string value, bool verbose = true)
        {
            String result = value + Char.MaxValue;
            return result.Select((c, i) =>
            {
                var ii = ((result.Length - 1) + i) % result.Length;
                var i_ = Convert.ToInt32(result[i]);
                var ii_ = Convert.ToInt32(result[ii]);
                return new int[] { i_, i, ii_, ii };
            })
            .OrderBy(i=>i,new BWTComparer());
        }
    }
}
