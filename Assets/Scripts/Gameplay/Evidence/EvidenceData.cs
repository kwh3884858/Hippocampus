using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Evidence
{
    [System.Serializable]
    public class EvidenceData
    {
        public List<string> evidenceList;

        public EvidenceData()
        {
            evidenceList = new List<string>();
        }
    }

    [System.Serializable]
    public class SingleEvidenceData
    {
        //public int id;
        public string exhibit;
        public string description;

        public SingleEvidenceData(/*int vId,*/ string vExhibit, string vDescription)
        {
            //id = vId;
            exhibit = vExhibit;
            description = vDescription;
        }
    }
}
