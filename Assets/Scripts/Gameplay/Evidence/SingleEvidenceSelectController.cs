using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;

namespace Evidence
{
    public class SingleEvidenceSelectController : UIPanel<UIDataProviderGameScene, DataProvider>
    {

        [SerializeField]
        private Text m_name = null;
        [SerializeField]
        private Text m_description = null;

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

        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        public void OnClickCloseButton()
        {
            // TODO:关闭当前UI
        }

    }
}
