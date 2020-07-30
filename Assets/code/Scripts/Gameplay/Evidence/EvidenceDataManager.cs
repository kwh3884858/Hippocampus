using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using StarPlatinum;
using StarPlatinum.Base;
using StarPlatinum.EventManager;
using UnityEngine;

namespace Evidence
{
    public class EvidenceDataManager : Singleton<EvidenceDataManager>
    {

        /// <summary>已经获得的证据</summary>
        public Dictionary<string, SingleEvidenceData> MyEvidenceDic { private set; get; }

        public EvidenceDataManager()
        {
            //LoadData();

        }

        public void Initialize()
        {
            LoadData();
            EventManager.Instance.AddEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
        }

        public void Shutdown()
        {
            EventManager.Instance.RemoveEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadData()
        {
            //MyEvidenceDic = new Dictionary<string, SingleEvidenceData>();
            //if (LocalData.HasKey(evidenceName))
            //{
            //    m_data = JsonConvert.DeserializeObject<EvidenceData>(LocalData.ReadStr(evidenceName, ""));
            //}
            //else
            //{
            //    m_data = new EvidenceData();
            //}
            m_evidenceConfig = ConfigData.Instance.evidenceConfig.GetDicDetails();
            //if (m_evidenceConfig != null)
            //{
            //    SingleEvidenceData evidenceData;
            //    foreach (var data in m_evidenceConfig)
            //    {
            //        if (m_data.evidenceList.ContainsKey(data.Key))
            //        {
            //            evidenceData = m_data.evidenceList[data.Key];
            //            //MyEvidenceDic.Add(data.Value.exhibit, new SingleEvidenceData(data.Value.exhibit, data.Value.description));
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 添加证据
        /// </summary>
        /// <param name="vId"></param>
        public void AddEvidence(string vExhibit)
        {
            if (MyEvidenceDic.ContainsKey(vExhibit))// 已经存在
            {
                return;
            }
            if (m_evidenceConfig.ContainsKey(vExhibit))
            {
                EvidenceConfig.Detail data = m_evidenceConfig[vExhibit];
                m_data.evidenceList.Add(vExhibit, new SingleEvidenceData(data.exhibit, data.description));
                MyEvidenceDic.Add(vExhibit, new SingleEvidenceData(data.exhibit, data.description));
                //SaveData();
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
                MyEvidenceDic.Remove(vExhibit);
                //SaveData();
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
            return MyEvidenceDic.ContainsKey(vExhibit) ? MyEvidenceDic[vExhibit] : null;
        }

        /// <summary>
        /// 保存信息到本地
        /// </summary>
        public void SaveData()
        {
            LocalData.WriteStr(evidenceName, JsonConvert.SerializeObject(m_data));
        }

        /// <summary>
        /// 设置证物id
        /// </summary>
        /// <param name="evidenceID">证物id</param>
        public void SetCorrectEvidenceID(string evidenceID)
        {
            m_curCorrectEvidenceID = evidenceID;
        }

        public bool IsCorrectEvidence(string evidenceID)
        {
            return m_curCorrectEvidenceID == evidenceID;
        }

        private void OnPlayerPreSaveArchive(object sender, PlayerPreSaveArchiveEvent e)
        {
            GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.EvidenceArchiveData.EvidenceList = MyEvidenceDic;
        }

        private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
        {
            MyEvidenceDic = GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.EvidenceArchiveData.EvidenceList;
            if(MyEvidenceDic == null)
            {
                MyEvidenceDic = new Dictionary<string, SingleEvidenceData>();
            }
            //m_isStoryTriggered = GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.MissionArchieData.StoryTriggered;
            //m_objectTriggeredCounter = GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.MissionArchieData.ObjectTriggeredCounter;
            //if (m_isStoryTriggered == null)
            //{
            //    m_isStoryTriggered = new List<string>();
            //}
            //if (m_objectTriggeredCounter == null)
            //{
            //    m_objectTriggeredCounter = new Dictionary<string, int>();
            //}
        }

        /// <summary>保存本地的名称</summary>
        private static string evidenceName = "Evidences";

        /// <summary>读取的本地配置文件信息</summary>
        private Dictionary<string, EvidenceConfig.Detail> m_evidenceConfig = null;
        /// <summary>本地保存的数据</summary>
        private EvidenceData m_data = new EvidenceData();
        /// <summary>当前正确的证据id</summary>
        private string m_curCorrectEvidenceID = "";
    }
}