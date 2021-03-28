using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class EvidenceStoryConfig: BaseConfig
    {
        public string ID;
        public string StoryId;
        public string LoadSceneNameOnEnd;
        public string LoadMissionIDOnEnd;

        public static EvidenceStoryConfig GetConfigByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            EvidenceStoryConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(EvidenceStoryConfig).Name}找不到Key:{key}");
            }
            return ret;
        }

        public static Dictionary<string, EvidenceStoryConfig> GetAllConfig()
        {
            return _configCache;
        }
        
        private static Dictionary<string, EvidenceStoryConfig> _configCache;

        public static void Init(Dictionary<string,EvidenceStoryConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}