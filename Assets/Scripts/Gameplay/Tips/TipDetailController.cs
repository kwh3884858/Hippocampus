using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tips
{
    /// <summary>
    /// Tip详情管理
    /// </summary>
    public class TipDetailController : MonoBehaviour
    {

        [SerializeField]
        private Text m_title = null;
        [SerializeField]
        private Text m_detail = null;
        [SerializeField]
        private GameObject m_tipLock = null;

        public void Init(TipData vData)
        {
            SetData(vData);
            if(m_data == null)
            {
                SetLockEnable(true);
            }
            else
            {
                Show();
            }
        }

        public void SetData(TipData vData)
        {
            m_data = vData;
        }

        public void Show()
        {
            SetLockEnable(false);
            SetDetailActive(true);
            if (m_title != null)
            {
                m_title.text = m_data.tip;
            }
            if (m_detail != null)
            {
                m_detail.text = m_data.description;
            }
        }

        public void Close()
        {
            SetDetailActive(false);
        }

        /// <summary>
        /// 显示锁定状态
        /// </summary>
        public void SetLockEnable(bool vEnable)
        {
            if(m_tipLock != null)
            {
                m_tipLock.SetActive(vEnable);
            }
        }

        private void SetDetailActive(bool vActive)
        {
            if (this.gameObject.activeSelf != vActive)
            {
                this.gameObject.SetActive(vActive);
            }
        }

        /// <summary>数据</summary>
        private TipData m_data = null;
    }
}
