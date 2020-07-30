using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using GamePlay.Stage;

namespace UI.Panels
{
	public partial class CommonGamePlayPanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);	
		}

		public override void DeInitialize()
		{
			m_model.Deactivate();
			base.DeInitialize();
		}

		public override void Hide()
		{
			m_model.Hide();
			base.Hide();
		}

		public override void Deactivate()
		{
			m_model.Deactivate();
			base.Deactivate();
		}

		public override void ShowData(DataProvider data)
		{
			m_model.ShowData(data);
			base.ShowData(data);
		}

		public override void UpdateData(DataProvider data)
		{
			m_model.UpdateData(data);
			base.UpdateData(data);
		}

		public override void Tick()
		{
			m_model.Tick();
			base.Tick();
		}

		public override void LateTick()
		{
			m_model.LateTick();
			base.LateTick();
		}

		public override void SubpanelChanged(UIPanelType type, DataProvider data = null)
		{
			m_model.SubpanelChanged(type, data);
			base.SubpanelChanged(type, data);
		}

		public override void SubpanelDataChanged(UIPanelType type, DataProvider data)
		{
			m_model.SubpanelDataChanged(type, data);
			base.SubpanelDataChanged(type, data);
		}
		#endregion

		#region Member
        public void OnClickMap()
        {
            Debug.Log("CommonGamePlayPanel show map");
            UIManager.Instance().ShowStaticPanel(UIPanelType.UIMapcanvasPanel);// 显示地图
        }

        public void OnClickAssistantInteraction()
        {
            // TODO: 助手互动
        }

        public void OnClickEvidence()
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonMapsTipsEvidencesPanel);// 显示证物列表
        }

        public void OnClickTips()
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.Tipspanel);// 显示tips列表
        }

		public void OnClickInteract ()
		{
			CoreContainer.Instance.EnablePlayerInteractability ();
		}

		#endregion
	}
}