using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Evidence
{
    public class EvidenceConfig
    {

        /// <summary>数据清单</summary>
        private Detail[] details = null;
        /// <summary>基于下限范围的对应清单</summary>
        private Dictionary<int, Detail> dic = new Dictionary<int, Detail>();
        /// <summary>有效信息量</summary>
        private int totalDetail = 0;

        public EvidenceConfig(Detail[] vDetails)
        {
            details = vDetails;
            if (details != null)
            {
                totalDetail = details.Length;
                int vCount = totalDetail;
                if (vCount > 0)
                {
                    int i;
                    Detail vSub;
                    for (i = 0; i < vCount; i += 1)
                    {
                        vSub = details[i];
                        if (!dic.ContainsKey(vSub.id))
                        {
                            dic.Add(vSub.id, vSub);
                        }
#if UNITY_EDITOR
                        else
                        {
                            Debug.LogWarningFormat("证据表的信息重复：{0}", vSub.id);
                        }
#endif
                    }
                }
            }
        }

        /// <summary>
        /// 提供指定编号的信息
        /// </summary>
        /// <param name="vId"></param>
        /// <returns></returns>
        public Detail GetDetail(int vId)
        {
            if (dic.ContainsKey(vId))
            {
                return dic[vId];
            }
            return null;
        }

        /// <summary>
        /// 提供清单的引用
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Detail> GetDicDetails()
        {
            return dic;
        }

        /// <summary>
        /// 细项配置
        /// </summary>
        [System.Serializable]
        public class Detail
        {

            /// <summary>证据id</summary>
            public int id;
            /// <summary>证据名称</summary>
            public string exhibit;
            /// <summary>描述</summary>
            public string description;
        }
    }
}

