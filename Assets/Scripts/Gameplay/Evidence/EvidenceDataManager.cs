using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Evidence
{
    public class EvidenceDataManager
    {

        public EvidenceDataManager()
        {
            LoadData();
        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadData()
        {
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
                m_data.evidenceList = new List<int>();
            }
            m_evidenceConfig = ConfigData.Instance.evidenceConfig.GetDicDetails();
            if (m_evidenceConfig != null)
            {
                int l = m_data.evidenceList.Count;
                int id;
                EvidenceConfig.Detail data;
                for (int i = 0; i < l; i++)
                {
                    id = m_data.evidenceList[i];
                    if (m_evidenceConfig.ContainsKey(id))
                    {
                        data = m_evidenceConfig[id];
                        m_myEvidenceDic.Add(data.id, new SingleEvidenceData(data.id, data.exhibit, data.description));
                    }
                }
            }
        }

        /// <summary>
        /// 添加证据
        /// </summary>
        /// <param name="vId"></param>
        public void AddEvidence(int vId)
        {
            if (m_evidenceConfig.ContainsKey(vId))
            {
                m_data.evidenceList.Add(vId);
                EvidenceConfig.Detail data = m_evidenceConfig[vId];
                m_myEvidenceDic.Add(data.id, new SingleEvidenceData(data.id, data.exhibit, data.description));
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
        public void RemoveEvidence(int vId)
        {
            if (m_evidenceConfig.ContainsKey(vId))
            {
                m_data.evidenceList.Remove(vId);
                m_myEvidenceDic.Remove(vId);
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
        public SingleEvidenceData GetEvidenceDetail(int vId)
        {
            return m_myEvidenceDic.ContainsKey(vId) ? m_myEvidenceDic[vId] : null;
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

        /// <summary>已经获得的证据</summary>
        private Dictionary<int, SingleEvidenceData> m_myEvidenceDic = null;
        /// <summary>读取的本地配置文件信息</summary>
        private Dictionary<int, EvidenceConfig.Detail> m_evidenceConfig = null;
        /// <summary>本地保存的数据</summary>
        private EvidenceData m_data = null;
    }
}