using System.Collections.Generic;
using UI.Panels;
using UI.Panels.Providers;
using UnityEngine;

namespace UI.Modules
{
    public class UIModuleMainMenu : UIModule<UIDataProviderMainMenu>
    {
        public override void Initialize(UIDataProvider uiDataProvider,Dictionary<UIPanelLayer,Transform> layer)
        {
            base.Initialize(uiDataProvider,layer);
            m_panelsSettings = UIPanelSettingProvider.MenuInfo;
        }
    }
}
