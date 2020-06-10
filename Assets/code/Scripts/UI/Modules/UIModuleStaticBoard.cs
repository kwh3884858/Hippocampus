using System;
using System.Collections.Generic;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UI.Panels.StaticBoard;

namespace UI.Modules
{
    public class UIModuleStaticBoard : UIModule<UIDataProviderBattle>
    {
        public override void Initialize(UIDataProvider uiDataProvider)
        {
            base.Initialize(uiDataProvider);
            m_panelsSettings = UIPanelSettingProvider.StaticBoardInfo;
        }
        public void ShowTalk(string talkID, Action onTalkEnd = null)
        {
            ShowPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = talkID, OnTalkEnd = onTalkEnd });
        }
    }
}
