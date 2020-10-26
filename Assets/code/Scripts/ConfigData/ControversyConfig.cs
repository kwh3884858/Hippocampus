using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class ControversyConfig : BaseConfig
    {
        public string ID;
        public string leftCharacterID;
        public int stageOneBarrageGroupID;
        public string stageOneWinStoryID;
        public string stageOneFailStoryID;
        public int stageTwoBarrageGroupID;
        public string stageTwoFailBackStoryID;
        public string stageTwoFailStoryID;
        public int specialBarrageID;
        public string winStoryID;
        public string breakTheoryImageKey;
        public string stageOneBgm;
        public string stageTwoBgm;
        public string storyFileName;
        public string entranceStoryID;

        public static ControversyConfig GetConfigByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            ControversyConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(ControversyConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<string, ControversyConfig> _configCache;

        public static void Init(Dictionary<string,ControversyConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}