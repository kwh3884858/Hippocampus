using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;

namespace Assistant
{
    /// <summary>
    /// 助手系统界面显示控制
    /// </summary>
    public class AssistantSystemPanel : UIPanel<UIDataProviderAssistant, DataProvider>
    {
        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
        }

        /// <summary>
        /// 显示时调用
        /// </summary>
        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
        }

        public void HidSelef()
        {
            base.InvokeHidePanel();
        }

        /// <summary>
        /// 点击了关闭按钮
        /// </summary>
        public void OnClickClose()
        {

        }
    }
}
