using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class CGScenePointConfig : BaseConfig
    {
        public int ID;
        public int posX;
        public int posY;
        public List<int> touchConfigIDs;

        public static CGScenePointConfig GetConfigByKey(int key)
        {
            CGScenePointConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(CGScenePointConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<int, CGScenePointConfig> _configCache;

        public static void Init(Dictionary<int,CGScenePointConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}