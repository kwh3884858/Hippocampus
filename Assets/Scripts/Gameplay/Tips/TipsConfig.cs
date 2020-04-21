using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tips
{
    public class TipsConfig
    {

        /// <summary>数据清单</summary>
        private Detail[] details = null;
        /// <summary>基于下限范围的对应清单</summary>
        private Dictionary<string, Detail> dic = new Dictionary<string, Detail>();

        public TipsConfig(Detail[] vDetails)
        {
            details = vDetails;
            if (details != null)
            {
                int vTotalDetail = details.Length;
                int vCount = vTotalDetail;
                if (vCount > 0)
                {
                    int i;
                    Detail vSub;
                    for (i = 0; i < vCount; i += 1)
                    {
                        vSub = details[i];
                        if (!dic.ContainsKey(vSub.tip))
                        {
                            dic.Add(vSub.tip, vSub);
                        }
#if UNITY_EDITOR
                        else
                        {
                            Debug.LogWarningFormat("tips表的信息重复：{0}", vSub.tip);
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
        public Detail GetDetail(string vTip)
        {
            if (dic.ContainsKey(vTip))
            {
                return dic[vTip];
            }
            return null;
        }

        /// <summary>
        /// 提供清单的引用
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Detail> GetDicDetails()
        {
            return dic;
        }

        /// <summary>
        /// 细项配置
        /// </summary>
        [System.Serializable]
        public class Detail
        {

            /// <summary>证据名称</summary>
            public string tip;
            /// <summary>描述</summary>
            public string description;
        }
    }
}

