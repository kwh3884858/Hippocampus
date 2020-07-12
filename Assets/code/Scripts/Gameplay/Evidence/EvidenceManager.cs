using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;

namespace Evidence
{
    /// <summary>
    /// 证物系统的总体控制
    /// </summary>
    public class EvidenceManager : Singleton<EvidenceManager>
    {

        /// <summary>
        /// 判断出示证据的结果
        /// </summary>
        /// <param name="evidence">证据</param>
        /// <returns></returns>
        public bool JudgePresentEvidenceResult(string evidence)
        {
            // TODO：需要获取正确的证物，然后进行判断
            return false;
        }

        //private EvidenceDataManager
    }
}

