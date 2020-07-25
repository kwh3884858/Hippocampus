using UI.Panels.Providers.DataProviders;
using UnityEngine;
namespace UI.Panels
{
    public class GameplayPromptWidgetModel: UIModel
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
			if (data != null) {
                PromptWidgetDataProvider promptWidgetData = data as PromptWidgetDataProvider;
                UpdateInteractableObject (promptWidgetData.m_interactiableObject);
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

        private void UpdateInteractableObject (GameObject interactableObject)
		{
            m_interactableObject = interactableObject;
		}
        #endregion

        #region Member

        public GameObject m_interactableObject { get; set; }
		
        #endregion
    }
}