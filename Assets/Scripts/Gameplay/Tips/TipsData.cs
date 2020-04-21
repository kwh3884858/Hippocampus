using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tips
{
    [System.Serializable]
    public class TipsData
    {
        //public List<TipData> tipsList;
        public Dictionary<string, TipData> tipsList;

        public TipsData()
        {
            //tipsList = new List<TipData>();
            tipsList = new Dictionary<string, TipData>();
        }
    }

    [System.Serializable]
    public class TipData
    {
        public string tip;
        public string description;
        /// <summary>是否已经解锁</summary>
        public bool isUnlock;
        /// <summary>获得时间</summary>
        public long time;
        /// <summary>是否已经点过</summary>
        public bool isAlreadyClick;

        public TipData(string vTip, string vDescription, bool vIsUnlock = false, long vTime = 0, bool vIsAlreadyClick = false)
        {
            tip = vTip;
            description = vDescription;
            isUnlock = vIsUnlock;
            time = vTime;
            isAlreadyClick = vIsAlreadyClick;
        }
    }
}
