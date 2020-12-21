using System.Collections.Generic;
using StarPlatinum;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Config.Data
{
    public class TalkPanelConfig : BaseConfig
    {
        public int ID;
        public string contentBG;
        public string nameBG;
        public string autoBtnBG;
        public string historyBtnBG;
        public string contentEndImg;
        public string talkBG;

        public static TalkPanelConfig GetConfigByKey(int key)
        {
            TalkPanelConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(TalkPanelConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<int, TalkPanelConfig> _configCache;

        public static void Init(Dictionary<int,TalkPanelConfig> configCache)
        {
            _configCache = configCache;
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }

        public static void Preload()
        {
            var list = new List<string>();
            foreach (var config in _configCache)
            {
                list.Add(config.Value.contentBG);
                list.Add(config.Value.contentEndImg);
                list.Add(config.Value.autoBtnBG);
                list.Add(config.Value.historyBtnBG);
                if (!string.IsNullOrEmpty(config.Value.talkBG))
                {
                    list.Add(config.Value.talkBG);
                }
                list.Add(config.Value.nameBG);
            }
            PrefabManager.Instance.LoadAssetsAsync<Sprite>(list,null);
        }
    }
}