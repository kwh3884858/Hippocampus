using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using System;
using UnityEngine.SceneManagement;
using GamePlay.Stage;

namespace UI.Panels
{
	public partial class CommonGamePlayTransitionPanel : UIPanel<UIDataProvider, DataProvider>
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
            if (m_model.m_animationTranstionType == AnimationType.FadeIn)
            {
				AnimationState state = m_img_Mask_Animation[m_fadeInAnimtionName];
				state.time = 0;
				UnityEngine.Assertions.Assert.IsTrue(state != null, "State can`t be nullptr");
				UnityEngine.Assertions.Assert.IsTrue(state.time != state.length, "State is 0 length");

				m_text_Intro_Text.text = "正在进入" + m_model.m_teleportedSceneName;

				if (m_img_Mask_Animation.Play(m_fadeInAnimtionName))
                {
					GamePlay.Player.PlayerController.Instance().SetMoveEnable(false);
					StartCoroutine(ClosePanel(state.length));
                }
            }
		}

        private IEnumerator ClosePanel(float length)
        {
			float timer = 0;
            while(timer < length)
            {
				timer += Time.deltaTime;
				yield return null;
            }
			GamePlay.Player.PlayerController.Instance().SetMoveEnable(true);
			InvokeHidePanel();
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
		public enum AnimationType
        {
			FadeIn,
        }
		private string m_fadeInAnimtionName = "UI_Common_GamePlay_Transition_Panel_FadeIn_Animation";

		#endregion
	}
}