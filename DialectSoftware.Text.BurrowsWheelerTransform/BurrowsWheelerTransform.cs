using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Text
{
    [Serializable]
    public sealed class BurrowsWheelerTransform:ISerializable 
    {
        private int[] index;
        private int[] suffix;
        private int[] sorted;
        private int[] bwt;
        private string text;

        public BurrowsWheelerTransform(String value) : this(value.ApplyBurrowsWheelerTransform(false))
        {
            text = value;
        }

        internal BurrowsWheelerTransform(IOrderedEnumerable<int[]> transform)
        {
            var y = transform.Select(v => new { F = v[0], L = v[2], S = v[1] }).ToArray();
            sorted = y.Select(v => v.F).ToArray();
            bwt = y.Select(v => v.L).ToArray();
            suffix = y.Select(v => v.S).ToArray();
            index = suffix.Select((s, i) => new { S = s, I = i }).OrderBy(v => v.S).Select(v => v.I).ToArray();
        }

        private BurrowsWheelerTransform(SerializationInfo info, StreamingContext context)
        {
            index = (int[])info.GetValue("index", typeof(int[]));
            suffix = (int[])info.GetValue("suffix", typeof(int[]));
            sorted = (int[])info.GetValue("sorted", typeof(int[]));
            bwt = (int[])info.GetValue("bwt", typeof(int[]));
            text = (string)info.GetValue("text", typeof(string));
        }

        public IEnumerable<int> Find(string query)
        {
            query = new String(query.Reverse().ToArray());
            IEnumerable<int> indecies = new int[] { };
            query.TakeWhile((c, i) =>
            {
                if (i == 0)
                {
                    indecies = Find(c);
                    return true;
                }
                else
                {
                    indecies = Find(indecies, c);
                    return true;
                }
            }).ToArray();
            return indecies.Select(i => suffix[i]).ToArray();
        }

        private IEnumerable<int> Find(char c)
        {
            return sorted
                .Select((s, i) => s == c ? i : -1)
                .Where(i => i > -1)
                .ToArray();
        }

        private IEnumerable<int> Find(IEnumerable<int> results, char c)
        {
            results = results.Where(r => bwt[r] == c).Select(i => i).ToArray();
            return results
               .Select((r) =>
               {
                   return index[suffix[r] - 1];
               }).ToArray();
        }

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("index",index);
            info.AddValue("suffix", suffix);
            info.AddValue("sorted", sorted);
            info.AddValue("bwt", bwt);
            info.AddValue("text", text);
        }

        public static implicit operator BurrowsWheelerTransform(string value)
        {
            return new BurrowsWheelerTransform(value);
        }

        public static implicit operator String(BurrowsWheelerTransform value)
        {
            return value.text;
        }
    }
}
