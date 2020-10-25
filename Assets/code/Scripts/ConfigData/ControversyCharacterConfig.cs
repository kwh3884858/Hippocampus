using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class ControversyCharacterConfig : BaseConfig
    {
        public string ID;
        public string entranceScreenKey;
        public string winScreenKey;
        public string loseScreenKey;
        public string IdleTachieKey;
        public string breakTheoryImgKey;

        public static ControversyCharacterConfig GetConfigByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            ControversyCharacterConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(ControversyCharacterConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<string, ControversyCharacterConfig> _configCache;

        public static void Init(Dictionary<string,ControversyCharacterConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}