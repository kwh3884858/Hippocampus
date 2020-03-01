using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Evidence
{
    /// <summary>
    /// 单个证据控制
    /// </summary>
    public class SingleEvidenceController : MonoBehaviour
    {
        /// <summary>证据数据</summary>
        private SingleEvidenceData m_data = null;

        public void Init(SingleEvidenceData vData)
        {
            SetData(vData);
            Show();
        }

        public void SetData(SingleEvidenceData vData)
        {
            m_data = vData;
        }

        public void Show()
        {
            // TODO：设置显示
            if (this.gameObject != null)
            {
                this.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 出示证据
        /// </summary>
        public void OnClick()
        {
            // TODO：调用证据显示UI
        }
    }
}
