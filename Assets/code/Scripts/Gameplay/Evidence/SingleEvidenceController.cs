using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UI.Panels.Providers.DataProviders;

namespace Evidence
{
    /// <summary>
    /// 单个证据控制
    /// </summary>
    public class SingleEvidenceController : MonoBehaviour
    {

        [SerializeField]
        private Text m_name = null;

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
            if (!m_data.isLock && this.gameObject != null)
            {
                this.gameObject.SetActive(true);
            }
            if (m_name != null)
            {
                m_name.text = m_data.exhibit;
            }
        }

        /// <summary>
        /// 出示证据
        /// </summary>
        public void OnClick()
        {
            // TODO：调用证据显示UI
            UIManager.Instance().ShowPanel(UIPanelType.Singleevidenceselectpanel, new EvidenceDataProvider() { Data = m_data });// 显示UI，wywtsest
        }
    }
}
