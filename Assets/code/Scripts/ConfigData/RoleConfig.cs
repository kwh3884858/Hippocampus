using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class RoleConfig : BaseConfig
    {
        public string roleID;
        public string artNameKey;
        public string typewriterSoundKey;

        public static RoleConfig GetConfigByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            RoleConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(RoleConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<string, RoleConfig> _configCache;

        public static void Init(Dictionary<string,RoleConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}