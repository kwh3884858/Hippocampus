﻿using System.Collections;
using System.Collections.Generic;
using StarPlatinum;
using StarPlatinum.EventManager;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
	public partial class CommonMainInterfacePanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);
			
			EventManager.Instance.AddEventListener<GameEvent>(GameEnd);
			
		}

		public override void DeInitialize()
		{
			m_model.Deactivate();
			base.DeInitialize();
			EventManager.Instance.RemoveEventListener<GameEvent>(GameEnd);

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

		private void GameEnd(object sender, GameEvent e)
		{
			switch (e.EventName)
			{
				case "GameEnd":
					InvokeShowPanel(UIPanelType.UICommonEndPanel);
					break;
			}
		}
		
		#region Member


		#endregion
	}
}