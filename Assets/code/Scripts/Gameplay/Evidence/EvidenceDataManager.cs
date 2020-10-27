using System.Collections;
using System.Collections.Generic;
using Controllers.Subsystems.Story;
using Newtonsoft.Json;
using StarPlatinum;
using StarPlatinum.Base;
using StarPlatinum.EventManager;
using UI.Panels.Providers;
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
        /// <param name="exhibitID">证物id</param>
        public bool AddEvidence(string exhibitID)
        {
            if (MyEvidenceDic.ContainsKey(exhibitID))// 已经存在
            {
                return false;
            }
            if (m_evidenceConfig.ContainsKey(exhibitID))
            {
                EvidenceConfig.Detail data = m_evidenceConfig[exhibitID];
                m_data.evidenceList.Add(exhibitID, new SingleEvidenceData(data.exhibitID,data.exhibit, data.description, data.exhibitImageName));
                MyEvidenceDic.Add(exhibitID, new SingleEvidenceData(data.exhibitID, data.exhibit, data.description, data.exhibitImageName));
                return true;
                //SaveData();
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("evidence id " + exhibitID + " not contain in exhibitTable!");
                return false;
            }
#endif
            return false;
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

        public string GetEvidenceWrongID(string evidenceID,string prefix)
        {
            var label = prefix + evidenceID + "_0";
            if (GameRunTimeData.Instance.ControllerManager.StoryController.IsLabelExist(label))
            {
                return label;
            }

            label = prefix + "_0";// "AskPhoto_0";
            if (GameRunTimeData.Instance.ControllerManager.StoryController.IsLabelExist(label))
            {
                return label;
            }
            return null;
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