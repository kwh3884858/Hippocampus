using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LocalCache;
using Newtonsoft.Json;
using UnityEngine;

namespace Config.Data
{
    public class ConfigDataProvider
    {
        public string m_dataPath = Application.streamingAssetsPath + "/Data/";
        public string m_jsonSuffix = ".json";
        public void InitialInfo()
        {
            LoadAllConfig();
        }

        private void LoadAllConfig()
        {
            RoleConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, RoleConfig>>(
                File.ReadAllText(m_dataPath + typeof(RoleConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            CGSceneConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, CGSceneConfig>>(
                File.ReadAllText(m_dataPath + typeof(CGSceneConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            CGScenePointConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, CGScenePointConfig>>(
                File.ReadAllText(m_dataPath + typeof(CGScenePointConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            CGScenePointTouchConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, CGScenePointTouchConfig>>(
                File.ReadAllText(m_dataPath + typeof(CGScenePointTouchConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            EvidenceConfig.Init(JsonConvert.DeserializeObject<NormalHGData>(
                File.ReadAllText(m_dataPath + typeof(EvidenceConfig).Name + m_jsonSuffix, Encoding.UTF8)));
            CommonConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, CommonConfig>>(
                File.ReadAllText(m_dataPath + typeof(CommonConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            ControversyConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, ControversyConfig>>(
                File.ReadAllText(m_dataPath + typeof(ControversyConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            ControversyBarrageConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, ControversyBarrageConfig>>(
                File.ReadAllText(m_dataPath + typeof(ControversyBarrageConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            ControversySpecialBarrageConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, ControversySpecialBarrageConfig>>(
                File.ReadAllText(m_dataPath + typeof(ControversySpecialBarrageConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            ControversyCharacterConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, ControversyCharacterConfig>>(
                File.ReadAllText(m_dataPath + typeof(ControversyCharacterConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
            TalkPanelConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, TalkPanelConfig>>(
                File.ReadAllText(m_dataPath + typeof(TalkPanelConfig).Name + m_jsonSuffix, Encoding.GetEncoding("GB2312"))));
        }


        public void Dispose()
        {
            RoleConfig.Dispose();
        }

    }
}
