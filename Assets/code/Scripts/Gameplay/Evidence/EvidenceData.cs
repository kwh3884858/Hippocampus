using System.Collections;
using System.Collections.Generic;
using Config.Data;
using UnityEngine;
// using UnityScript.Scripting.Pipeline; // compiler error, unity upgrade to 2021.3.4

namespace Evidence
{
    /// <summary>
    /// 本地 证据
    /// </summary>
    [System.Serializable]
    public class EvidenceData
    {
        //public List<string> evidenceList;
        public Dictionary<string, SingleEvidenceData> evidenceList;

        public EvidenceData()
        {
            //evidenceList = new List<string>();
            evidenceList = new Dictionary<string, SingleEvidenceData>();
        }
    }

    /// <summary>
    /// 单个证据信息 保存的
    /// </summary>
    [System.Serializable]
    public class SingleEvidenceData
    {
        //public int id;
        public string exhibitID;
        public string exhibit;
        public string description;
        /// <summary>详情图片</summary>
        public string exhibitImageName;
        /// <summary>是否锁定</summary>
        //public bool isLock;

        public SingleEvidenceData(string exhibitID, string vExhibit, string vDescription, string exhibitImageName)
        {
            this.exhibitID = exhibitID;
            exhibit = vExhibit;
            description = vDescription;
            this.exhibitImageName = exhibitImageName;
        }
    }

    public class EvidenceStoryInfo
    {
        public List<string> lackEvidenceID;
        public EvidenceStoryConfig config;

        public EvidenceStoryInfo(EvidenceStoryConfig config)
        {
            lackEvidenceID = new List<string>();
            config = config;
            var evidences = config.ID.Split(new[] {'|'});
            for (int i = 0; i < evidences.Length; i++)
            {
                if(!EvidenceDataManager.Instance.IsEvidenceExist(evidences[i]))
                {
                    lackEvidenceID.Add(evidences[i]);
                }
            }
        }
    }
}
