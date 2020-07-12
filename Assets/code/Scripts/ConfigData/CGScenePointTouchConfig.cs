using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class CGScenePointTouchConfig: BaseConfig
    {
        public int ID;
        public int touchNum;
        public string storyID;
        public string mouseEffectKey;

        public static CGScenePointTouchConfig GetConfigByKey(int key)
        {
            CGScenePointTouchConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(CGScenePointTouchConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<int, CGScenePointTouchConfig> _configCache;

        public static void Init(Dictionary<int,CGScenePointTouchConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}