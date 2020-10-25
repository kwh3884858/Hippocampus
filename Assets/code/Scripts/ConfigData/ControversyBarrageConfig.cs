using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class ControversyBarrageConfig : BaseConfig
    {
        public int ID;
        public int groupID;
        public string text;
        public int correctIndex;
        public int bornTime;
        public int track;

        public static ControversyBarrageConfig GetConfigByKey(int key)
        {
            ControversyBarrageConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(ControversyBarrageConfig).Name}找不到Key:{key}");
            }
            return ret;
        }

        public static Dictionary<int, ControversyBarrageConfig> GetAllConfig()
        {
            return _configCache;
        }
        
        private static Dictionary<int, ControversyBarrageConfig> _configCache;

        public static void Init(Dictionary<int,ControversyBarrageConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}