using UI.Panels;
using UI.Panels.Providers;

namespace UI.Modules
{
    public class UIModuleMainMenu : UIModule<UIDataProviderMainMenu>
    {
        public override void Initialize(UIDataProvider uiDataProvider)
        {
            base.Initialize(uiDataProvider);
            m_panelsSettings = UIPanelSettingProvider.MenuInfo;
        }
    }
}
