using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI;

namespace Evidence
{
    public class SingleEvidenceSelectController : UIPanel<UIDataProviderEvidence, EvidenceDataProvider>
    {

        [SerializeField]
        private Text m_name = null;
        [SerializeField]
        private Text m_description = null;

        /// <summary>证据数据</summary>
        private SingleEvidenceData m_data = null;

        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            Init(UIPanelDataProvider.Data, UIPanelDataProvider.CloseEvidenceUI, UIPanelDataProvider.OnShowEvidence);
        }

        public void Init(SingleEvidenceData vData, System.Action closeEvidenceUI, System.Action onShowEvidence)
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
            if (m_data != null)
            {
                if (m_name != null)
                {
                    m_name.text = m_data.exhibit;
                }
                if (m_description != null)
                {
                    m_description.text = m_data.description;
                }
            }
        }

        /// <summary>
        /// 点击出示证据按钮
        /// </summary>
        public void OnClickShowButton()
        {
            // 目前由证物系统对错误结果进行处理
            if (EvidenceDataManager.Instance.IsCorrectEvidence(m_data.exhibit))
            {
                m_closeEvidenceUI?.Invoke();
                m_closeEvidenceUI = null;
                m_onShowEvidence?.Invoke();
                m_onShowEvidence = null;
            }
            else
            {
                // TODO: 提示选择错误
            }
            InvokeHidePanel();
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        public void OnClickCloseButton()
        {
            // TODO:关闭当前UI
            base.InvokeHidePanel();
        }

        private System.Action m_closeEvidenceUI = null;
        private System.Action m_onShowEvidence = null;

    }
}
