using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
    public class CommonGamePlayTransitionModel: UIModel
    {
        #region template method
        public override void Initialize(IUiPanel uiPanel )
        {
            base.Initialize(uiPanel);
        }

        public override void DeInitialize()
        {
            base.DeInitialize();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
        }

        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);

            if (data != null)
            {
                if (data is CommonGamePlayTransitionDataProvider)
                {
                    CommonGamePlayTransitionDataProvider commonGamePlayDataProvider = data as CommonGamePlayTransitionDataProvider;
                    m_animationTranstionType = commonGamePlayDataProvider.m_animationTranstionType;
                }
            }
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void LateTick()
        {
            base.LateTick();
        }

        public override void SubpanelChanged(UIPanelType type, DataProvider data = null)
        {
            base.SubpanelChanged(type,data);
        }

        public override void SubpanelDataChanged(UIPanelType type, DataProvider data)
        {
            base.SubpanelDataChanged(type,data);
        }
        #endregion

        #region Member
        public CommonGamePlayTransitionPanel.AnimationType m_animationTranstionType;
        #endregion
    }
}