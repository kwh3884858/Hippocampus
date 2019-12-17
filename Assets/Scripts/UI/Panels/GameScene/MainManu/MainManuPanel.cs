using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;

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
            InvokeChangePanel(UIPanelType.HudPanel,new HudDataProvider(){Data = 19});

        }

        private int i = 0;
    }
}