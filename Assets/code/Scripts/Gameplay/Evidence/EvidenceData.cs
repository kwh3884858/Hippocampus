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
        public string exhibit;
        public string description;
        /// <summary>是否锁定</summary>
        public bool isLock;

        public SingleEvidenceData(/*int vId,*/ string vExhibit, string vDescription, bool vIsLock = true)
        {
            //id = vId;
            exhibit = vExhibit;
            description = vDescription;
            isLock = vIsLock;
        }
    }
}
