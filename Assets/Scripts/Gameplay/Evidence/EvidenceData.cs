using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Evidence
{
    [System.Serializable]
    public class EvidenceData
    {
        public List<int> evidenceList;

        public EvidenceData()
        {
            evidenceList = new List<int>();
        }
    }

    [System.Serializable]
    public class SingleEvidenceData
    {
        public int id;
        public string exhibit;
        public string description;

        public SingleEvidenceData(int vId, string vExhibit, string vDescription)
        {
            id = vId;
            exhibit = vExhibit;
            description = vDescription;
        }
    }
}
