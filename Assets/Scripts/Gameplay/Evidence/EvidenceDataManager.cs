using System.Collections;
using System.Collections.Generic;
using StarPlatinum;
using UnityEngine;

namespace Evidence
{
    public class EvidenceDataManager : Singleton<EvidenceDataManager>
    {

        /// <summary>已经获得的证据</summary>
        public Dictionary<string, SingleEvidenceData> myEvidenceDic { private set; get; } = null;

        public EvidenceDataManager()
        {
            LoadData();
        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadData()
        {
            myEvidenceDic = new Dictionary<string, SingleEvidenceData>();
            if (LocalData.HasKey(evidenceName))
            {
                m_data = JsonUtility.FromJson<EvidenceData>(LocalData.ReadStr(evidenceName, ""));
            }
            else
            {
                m_data = new EvidenceData();
            }

            if (m_data.evidenceList == null)
            {
                m_data.evidenceList = new List<string>();
            }
            m_evidenceConfig = ConfigData.Instance.evidenceConfig.GetDicDetails();
            if (m_evidenceConfig != null)
            {
                int l = m_data.evidenceList.Count;
                string id;
                EvidenceConfig.Detail data;
                for (int i = 0; i < l; i++)
                {
                    id = m_data.evidenceList[i];
                    if (m_evidenceConfig.ContainsKey(id))
                    {
                        data = m_evidenceConfig[id];
                        myEvidenceDic.Add(data.exhibit, new SingleEvidenceData(/*data.id, */data.exhibit, data.description));
                    }
                }
            }
        }

        /// <summary>
        /// 添加证据
        /// </summary>
        /// <param name="vId"></param>
        public void AddEvidence(string vExhibit)
        {
            if (myEvidenceDic.ContainsKey(vExhibit))// 已经存在
            {
                return;
            }
            if (m_evidenceConfig.ContainsKey(vExhibit))
            {
                m_data.evidenceList.Add(vExhibit);
                EvidenceConfig.Detail data = m_evidenceConfig[vExhibit];
                myEvidenceDic.Add(vExhibit, new SingleEvidenceData(/*data.id, */data.exhibit, data.description));
                SaveData();
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log("evidence id not contain in exhibitTable!");
            }
#endif
        }

        /// <summary>
        /// 移除证据
        /// </summary>
        /// <param name="vId"></param>
        public void RemoveEvidence(string vExhibit)
        {
            if (m_evidenceConfig.ContainsKey(vExhibit))
            {
                m_data.evidenceList.Remove(vExhibit);
                myEvidenceDic.Remove(vExhibit);
                SaveData();
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log("evidence id not contain in exhibitTable!");
            }
#endif
        }

        /// <summary>
        /// 获取指定id的证据的信息
        /// </summary>
        /// <param name="vId"></param>
        /// <returns></returns>
        public SingleEvidenceData GetEvidenceDetail(string vExhibit)
        {
            return myEvidenceDic.ContainsKey(vExhibit) ? myEvidenceDic[vExhibit] : null;
        }

        /// <summary>
        /// 保存信息到本地
        /// </summary>
        public void SaveData()
        {
            LocalData.WriteStr(evidenceName, JsonUtility.ToJson(m_data));
        }

        /// <summary>保存本地的名称</summary>
        private static string evidenceName = "Evidence";

        /// <summary>读取的本地配置文件信息</summary>
        private Dictionary<string, EvidenceConfig.Detail> m_evidenceConfig = null;
        /// <summary>本地保存的数据</summary>
        private EvidenceData m_data = new EvidenceData();
    }
}