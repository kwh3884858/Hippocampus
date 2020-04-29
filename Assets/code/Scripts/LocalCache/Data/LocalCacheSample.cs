using System.Collections.Generic;
using System;

namespace LocalCache
{
    [Serializable]
    public class Sample
    {
        public int a;
        public double b;
    }

    public class SampleInfo : LocalCacheBase
    {
        public long a { get; set; }
        public List<Sample> b { get; set; }
        public Dictionary<int, long> c { get; set; }
        public string d { get; set; }
        public int e { get; set; }

        public SampleInfo()
        {
            b = new List<Sample>();
            c = new Dictionary<int, long>();
        }
    }
}