using System;
using System.Collections.Generic;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UI.Panels.StaticBoard;
using UnityEngine;

namespace UI.Modules
{
    public class UIModuleStaticBoard : UIModule<UIDataProviderBattle>
    {
        public override void Initialize(UIDataProvider uiDataProvider,Dictionary<UIPanelLayer,Transform> layer)
        {
            base.Initialize(uiDataProvider,layer);
            m_panelsSettings = UIPanelSettingProvider.StaticBoardInfo;
        }
        public void ShowTalk(string talkID, Action onTalkEnd = null)
        {
            ShowPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = talkID, OnTalkEnd = onTalkEnd });
        }
    }
}
