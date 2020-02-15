using System.Collections.Generic;
using System;

namespace LocalCache
{

    public class DictionaryCache : LocalCacheBase
    {
        public Dictionary<string, int> IntDic { get; set; }
        public Dictionary<string, float> FloatDic { get; set; }
        public Dictionary<string, string> StringDic { get; set; }


        public DictionaryCache()
        {
            IntDic = new Dictionary<string, int>();
            FloatDic = new Dictionary<string, float>();
            StringDic = new Dictionary<string, string>();
        }
    }
}