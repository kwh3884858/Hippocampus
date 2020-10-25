using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class ControversySpecialBarrageConfig : BaseConfig
    {
        public int ID;
        public int track;
        public string text;
        public int correctIndex;
        public string missStoryID;
        public string wrongStoryID1;
        public string wrongStoryID2;
        public string wrongStoryID3;
        public string wrongStoryID4;
        public int bornTime;

        public static ControversySpecialBarrageConfig GetConfigByKey(int key)
        {

            ControversySpecialBarrageConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(ControversySpecialBarrageConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<int, ControversySpecialBarrageConfig> _configCache;

        public static void Init(Dictionary<int,ControversySpecialBarrageConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}