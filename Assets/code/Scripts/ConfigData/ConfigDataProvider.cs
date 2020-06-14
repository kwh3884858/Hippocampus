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
        public void InitialInfo()
        {
            LoadAllConfig();
        }

        private void LoadAllConfig()
        {
            RoleConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, RoleConfig>>(
                File.ReadAllText(m_dataPath + typeof(RoleConfig).Name + ".json")));
        }
        

        public void Dispose()
        {
            RoleConfig.Dispose();
        }
        
    }
}