using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class CGSceneConfig: BaseConfig
    {
        public string ID;
        public List<int> pointIDs;
        public string CGItemKey;
        public string StoryFileName;
        public string LoadSceneNameOnEnd;
        public string LoadMissionIDOnEnd;
        public string ShowLabelOnEndStoryFileName;
        public string ShowLabelOnEnd;

        public static CGSceneConfig GetConfigByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            CGSceneConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(CGSceneConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<string, CGSceneConfig> _configCache;

        public static void Init(Dictionary<string,CGSceneConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}