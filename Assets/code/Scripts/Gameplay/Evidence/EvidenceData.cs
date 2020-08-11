using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
