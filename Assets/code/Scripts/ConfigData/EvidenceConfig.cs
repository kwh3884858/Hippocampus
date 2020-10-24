using System.Collections.Generic;
using UnityEngine;

namespace Config.Data
{
    public class EvidenceConfig : BaseConfig
    {
        public string description;
        public string exhibit;
        public string exhibitID;
        public string exhibitImageName;

        public static EvidenceConfig GetConfigByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            EvidenceConfig ret;
            if (!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(EvidenceConfig).Name}找不到Key:{key}");
            }
            return ret;
        }

        private static Dictionary<string, EvidenceConfig> _configCache;
        private static NormalHGData _normalHGData;

        public static void Init(Dictionary<string, EvidenceConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Init(NormalHGData normalHGData)
        {
            _normalHGData = normalHGData;
            if (_normalHGData != null)
            {
                List<EvidenceConfig> values = _normalHGData.value;
                if (values != null)
                {
                    int l = values.Count;
                    _configCache = new Dictionary<string, EvidenceConfig>();
                    for (int i = 0; i < l; i++)
                    {
                        _configCache.Add(values[i].exhibitID, values[i]);
                    }
                }
            }
        }

        public static void Dispose()
        {
            _configCache.Clear();
            _normalHGData = null;
        }
    }
}