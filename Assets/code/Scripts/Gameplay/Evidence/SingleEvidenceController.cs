using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UI.Panels.Providers.DataProviders;
using System;

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
        private EvidencesController m_father = null;

        public void Init(EvidencesController father, SingleEvidenceData vData, Action closeEvidenceUI, Action<string> onShowEvidence = null)
        {
            SetData(vData);
            m_father = father;
            m_closeEvidenceUI = closeEvidenceUI;
            m_onShowEvidence = onShowEvidence;
            Show();
        }

        public void Init(SingleEvidenceData vData, Action closeEvidenceUI, Action<string> onShowEvidence = null)
        {
            SetData(vData);
            m_closeEvidenceUI = closeEvidenceUI;
            m_onShowEvidence = onShowEvidence;
            Show();
        }

        public void SetData(SingleEvidenceData vData)
        {
            m_data = vData;
        }

        public void Show()
        {
            // TODO：设置显示
            if (/*!m_data.isLock && */this.gameObject != null)
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
            //// TODO：调用证据显示UI
            //UIManager.Instance().ShowStaticPanel(UIPanelType.Singleevidenceselectpanel,
            //    new EvidenceDataProvider()
            //    {
            //        Data = m_data,
            //        CloseEvidenceUI = m_closeEvidenceUI,
            //        OnShowEvidence = m_onShowEvidence
            //    });// 显示UI，wywtsest
            // 显示简述
            if (m_father != null)
            {
                m_father.RefreshIntroView(m_data);
            }
        }

        private Action m_closeEvidenceUI = null;
        private Action<string> m_onShowEvidence = null;
    }
}
