using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
    public class CommonGamePlayModel: UIModel
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
                if (data is CommonGamePlayDataProvider)
                {
                    CommonGamePlayDataProvider commonGamePlayDataProvider = data as CommonGamePlayDataProvider;
                    m_isInteractButtonVisiable = commonGamePlayDataProvider.m_interactButtonShouldVisiable;
                    m_itemName = commonGamePlayDataProvider.m_itemName;
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
        public bool m_isInteractButtonVisiable = false;
        public string m_itemName;
        #endregion
    }
}