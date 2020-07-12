using System;
using System.Collections.Generic;
using System.IO;
using LocalCache;
using Newtonsoft.Json;
using UnityEngine;

namespace Config.Data
{
    public class ConfigDataProvider
    {
        public string m_dataPath="Assets/data/configData/";
        public string m_jsonSuffix = ".json";
        public void InitialInfo()
        {
            LoadAllConfig();
        }

        private void LoadAllConfig()
        {
            RoleConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, RoleConfig>>(
                File.ReadAllText(m_dataPath + typeof(RoleConfig).Name + m_jsonSuffix)));
            CGSceneConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, CGSceneConfig>>(
                File.ReadAllText(m_dataPath + typeof(CGSceneConfig).Name + m_jsonSuffix)));
            CGScenePointConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, CGScenePointConfig>>(
                File.ReadAllText(m_dataPath + typeof(CGScenePointConfig).Name + m_jsonSuffix)));
            CGScenePointTouchConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, CGScenePointTouchConfig>>(
                File.ReadAllText(m_dataPath + typeof(CGScenePointTouchConfig).Name + m_jsonSuffix)));
        }
        

        public void Dispose()
        {
            RoleConfig.Dispose();
        }
        
    }
}