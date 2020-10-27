using System;
using System.Collections;
using System.Collections.Generic;
using StarPlatinum;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.StaticBoard;

namespace UI.Panels
{
	public enum EnumBreakTheoryType
	{
		CutIn,
		BreakTheory,
		Theory,
	}
	public partial class CommonBreakTheoryPanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);	
			gameObject.SetActive(false);
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
			var uiData = data as BreakTheoryDataProvider;
			gameObject.SetActive(true);
			SetInfo(uiData);
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

		private void SetInfo(BreakTheoryDataProvider dataProvider)
		{
			switch (dataProvider.Type)
			{
				case EnumBreakTheoryType.Theory:
					m_pl_breakTheory.gameObject.SetActive(true);
					m_go_theory_Animation.gameObject.SetActive(true);
					m_go_breakTheory_Animation.gameObject.SetActive(false);
					m_img_bg_Image.gameObject.SetActive(false);
					PrefabManager.Instance.SetImage(m_img_theory_Image,dataProvider.ImgKey);
					break;
				case EnumBreakTheoryType.BreakTheory:
					m_pl_breakTheory.gameObject.SetActive(true);
					m_go_theory_Animation.gameObject.SetActive(false);
					m_go_breakTheory_Animation.gameObject.SetActive(true);
					PrefabManager.Instance.SetImage(m_img_theoryBreak_Image,dataProvider.ImgKey);
					m_img_bg_Image.gameObject.SetActive(false);
					break;
				case EnumBreakTheoryType.CutIn:
					m_pl_breakTheory.gameObject.SetActive(false);
					m_img_bg_Image.gameObject.SetActive(true);
					PrefabManager.Instance.SetImage(m_img_bg_Image,dataProvider.ImgKey);
					break;
			}
			m_closeCallback = dataProvider.CloseCallback;
			
			CallbackTime(m_animationTime, () =>
			{
				InvokeHidePanel();
				m_closeCallback?.Invoke();
			});
		}
		
		#region Member

		private float m_animationTime = 2;
		private Action m_closeCallback;

		#endregion
	}
}