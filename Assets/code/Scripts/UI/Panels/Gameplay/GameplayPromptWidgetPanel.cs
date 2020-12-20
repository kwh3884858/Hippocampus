using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using System;
using StarPlatinum.Services;

namespace UI.Panels
{
	public partial class GameplayPromptWidgetPanel : UIPanel<UIDataProvider, DataProvider>
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

		private void UpdateWidgetPosition ()
		{
			GameObject interactable = m_model.m_interactableObject;
			if (interactable != null) {
				Vector3 screenPos = CameraService.Instance.GetMainCamera ().GetComponent<Camera> ().WorldToScreenPoint (interactable.transform.position);
				m_PromptWidget_Image_Image.transform.position = screenPos;
			}
		}

		public override void Tick()
		{
			m_model.Tick();
			base.Tick();
			UpdateWidgetPosition ();
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


		#endregion
	}
}