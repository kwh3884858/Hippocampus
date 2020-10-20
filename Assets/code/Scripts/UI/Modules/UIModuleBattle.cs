using System.Collections.Generic;
using UI.Panels;
using UI.Panels.Providers;
using UnityEngine;

namespace UI.Modules
{
    public class UIModuleBattle : UIModule<UIDataProviderBattle>
    {
        public override void Initialize(UIDataProvider uiDataProvider,Dictionary<UIPanelLayer,Transform> layers)
        {
            base.Initialize(uiDataProvider,layers);
            m_panelsSettings = UIPanelSettingProvider.BattleInfo;
        }
    }
}
