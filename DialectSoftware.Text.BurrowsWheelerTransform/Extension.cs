using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

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
