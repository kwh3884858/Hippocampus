using UI.Panels;
using UI.Panels.Providers;

namespace UI.Modules
{
    public class UIModuleBattle : UIModule<UIDataProviderBattle>
    {
        public override void Initialize(UIDataProvider uiDataProvider)
        {
            base.Initialize(uiDataProvider);
            m_panelsSettings = UIPanelSettingProvider.BattleInfo;
        }
    }
}
