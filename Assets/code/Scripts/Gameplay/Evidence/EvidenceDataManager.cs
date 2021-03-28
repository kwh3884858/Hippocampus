using System.Collections;
using System.Collections.Generic;
using Config.Data;
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
                AfterGetEvidence(exhibitID);
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

        public void RemoveAllEvidence()
        {
            m_data.evidenceList.Clear();
            MyEvidenceDic.Clear();
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

        public bool IsEvidenceExist(string evidenceName)
        {
            return MyEvidenceDic.ContainsKey(evidenceName);
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
            GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.EvidenceArchiveData.RemovedEvidenceStory = m_removedEvidenceStoryId;
        }

        private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
        {
            var data = GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData
                .EvidenceArchiveData;
            MyEvidenceDic = data.EvidenceList;
            m_removedEvidenceStoryId = data.RemovedEvidenceStory;
            if(MyEvidenceDic == null)
            {
                MyEvidenceDic = new Dictionary<string, SingleEvidenceData>();
            }
            if (m_removedEvidenceStoryId == null)
            {
                m_removedEvidenceStoryId = new List<string>();
            }
            InitEvidenceStoryInfo();
        }


        #region 证物剧情

        private List<string> m_removedEvidenceStoryId;
        private Dictionary<string,EvidenceStoryInfo> m_evidenceStoryInfos = new Dictionary<string, EvidenceStoryInfo>();
        private List<EvidenceStoryInfo> m_triggerEvidenceStory = new List<EvidenceStoryInfo>();
        private void InitEvidenceStoryInfo()
        {
            m_evidenceStoryInfos.Clear();
            var configs = EvidenceStoryConfig.GetAllConfig();
            foreach (var evidenceStoryConfig in configs)
            {
                if (m_removedEvidenceStoryId.Contains(evidenceStoryConfig.Key))
                {
                    continue;
                }

                var info = new EvidenceStoryInfo(evidenceStoryConfig.Value);
                if (info.lackEvidenceID.Count == 0)
                {
                    m_triggerEvidenceStory.Add(info);
                    continue;
                }
                m_evidenceStoryInfos.Add(evidenceStoryConfig.Key,info);
            }
        }

        /// <summary>
        /// 获取证物后检测是否激活剧情
        /// </summary>
        /// <param name="evidenceId"></param>
        private void AfterGetEvidence(string evidenceId)
        {
            List<string> removeEvidenceStoryInfos = new List<string>();
            foreach (var info in m_evidenceStoryInfos)
            {
                if (info.Value.lackEvidenceID.Remove(evidenceId)&& info.Value.lackEvidenceID.Count==0)
                {
                    removeEvidenceStoryInfos.Add(info.Key);
                }
            }

            foreach (var info in removeEvidenceStoryInfos)
            {
                m_triggerEvidenceStory.Add(m_evidenceStoryInfos[info]);
                EventManager.Instance.SendEvent(EventKey.EventStoryTrigger,m_evidenceStoryInfos[info]);
                m_evidenceStoryInfos.Remove(info);
            }
        }

        public EvidenceStoryInfo GetTriggeredEvidenceStory()
        {
            EvidenceStoryInfo info = null;
            if (m_triggerEvidenceStory.Count > 0)
            {
                info = m_triggerEvidenceStory[0];
                m_triggerEvidenceStory.RemoveAt(0);
            }
            return info;
        }

        public void RemoveEvidenceStory(EvidenceStoryInfo storyInfo)
        {
            m_triggerEvidenceStory.Remove(storyInfo);
        }

        #endregion
        

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