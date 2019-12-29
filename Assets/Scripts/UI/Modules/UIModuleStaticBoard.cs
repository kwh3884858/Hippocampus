using System;
using System.Collections.Generic;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UI.Panels.StaticBoard;

namespace UI.Modules
{
    public class UIModuleStaticBoard : UIModule<UIDataProviderBattle>
    {
        //
        public void ShowOptions(List<Option> options, Action<string> callback)
        {
            ShowPanel(UIPanelType.OptionsPanel,new OptionsDataProvider(){ Options = options,Callback = callback});
        }

        public void ShowTalk(string talkID, Action onTalkEnd = null)
        {
            ShowPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = talkID, OnTalkEnd = onTalkEnd });
        }
    }
}
