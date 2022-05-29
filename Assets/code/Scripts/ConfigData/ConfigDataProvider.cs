using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LocalCache;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Config.Data
{
    public class ConfigDataProvider : MonoBehaviour
    {
        public string m_dataPath = Application.streamingAssetsPath + "/Data/";
        public string m_jsonSuffix = ".json";

        public IEnumerator LoadAllConfig()
        {
            yield return StartCoroutine(LoadConfig<RoleConfig>((file) =>
                {
                    RoleConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, RoleConfig>>(file));
                }));
            yield return StartCoroutine(LoadConfig<CGSceneConfig>((file) =>
            {
                CGSceneConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, CGSceneConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<CGScenePointConfig>((file) =>
            {
                CGScenePointConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, CGScenePointConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<CGScenePointTouchConfig>((file) =>
            {
                CGScenePointTouchConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, CGScenePointTouchConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<EvidenceConfig>((file) =>
            {
                EvidenceConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, EvidenceConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<CommonConfig>((file) =>
            {
                CommonConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, CommonConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<ControversyConfig>((file) =>
            {
                ControversyConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, ControversyConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<ControversyBarrageConfig>((file) =>
            {
                ControversyBarrageConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, ControversyBarrageConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<ControversySpecialBarrageConfig>((file) =>
            {
                ControversySpecialBarrageConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, ControversySpecialBarrageConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<ControversyCharacterConfig>((file) =>
            {
                ControversyCharacterConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, ControversyCharacterConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<TalkPanelConfig>((file) =>
            {
                TalkPanelConfig.Init(JsonConvert.DeserializeObject<Dictionary<int, TalkPanelConfig>>(file));
            }));
            yield return StartCoroutine(LoadConfig<EvidenceStoryConfig>((file) =>
            {
                EvidenceStoryConfig.Init(JsonConvert.DeserializeObject<Dictionary<string, EvidenceStoryConfig>>(file));
            }));
        }

        private IEnumerator LoadConfig<T>(Action<string> callback) where T : BaseConfig
        {
            string fileStr = "";
            string filePath = m_dataPath + typeof(T).Name + m_jsonSuffix;
            UnityWebRequest req = UnityWebRequest.Get(filePath);

            yield return req.SendWebRequest();

            if (req.isHttpError||req.isNetworkError)
                Debug.LogError($"获取配置资源错误! {filePath}");
            else
            {
                fileStr =Encoding.GetEncoding("utf-8").GetString(req.downloadHandler.data);
            }
            callback?.Invoke(fileStr);
            yield break;
        }
        public void Dispose()
        {
            RoleConfig.Dispose();
        }

    }
}
