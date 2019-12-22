using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;

namespace UI.Panels.GameScene.MainManu
{
    public class MainManuPanel : UIPanel<UIDataProviderGameScene,DataProvider>
    {
        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
            CallbackTime(1,ShowHud);
        }
        

        public void ShowHud()
        {
            InvokeChangePanel(UIPanelType.TalkPanel,new TalkDataProvider(){ID = "1"});
        }

        private int i = 0;
    }
}