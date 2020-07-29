using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class EvidenceConfig: BaseConfig
    {
        public string ID;
        public string imagePath;

        public static EvidenceConfig GetConfigByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            EvidenceConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(EvidenceConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<string, EvidenceConfig> _configCache;

        public static void Init(Dictionary<string, EvidenceConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}