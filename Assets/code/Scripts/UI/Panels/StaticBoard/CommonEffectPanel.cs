using System;
using System.Collections;
using System.Collections.Generic;
using StarPlatinum;
using StarPlatinum.EventManager;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.StaticBoard;

namespace UI.Panels
{
	public enum EnumUIEffectType
	{
		Flash
	}
	
	public class UIEffectData
	{
		public EnumUIEffectType Type;
		public Action EndCallback;
	}
	
	public partial class CommonEffectPanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);	
			GetAllAnimationClipLength();
			EventManager.Instance.AddEventListener(EventKey.ShowUIEffect, OnShowUIEffect);
		}

		public override void DeInitialize()
		{
			EventManager.Instance.RemoveEventListener(EventKey.ShowUIEffect, OnShowUIEffect);
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

		private void ShowEffect(EnumUIEffectType type)
		{
			switch (type)
			{
				case EnumUIEffectType.Flash:
					m_pl_root_Animator.Play(m_flashAnimationName,-1,0);
					break;
			}
		}

		private void OnShowUIEffect(object data)
		{
			if (data is UIEffectData effectData)
			{
				ShowEffect(effectData.Type);
				if (effectData.EndCallback != null)
				{
					CallbackTime(m_animationClipLength[effectData.Type],effectData.EndCallback);
				}
			}
		}

		private void GetAllAnimationClipLength()
		{
			AnimationClip[] clips = m_pl_root_Animator.runtimeAnimatorController.animationClips;
			foreach(AnimationClip clip in clips)
			{
				switch (clip.name)
				{
					case m_flashAnimationName:
						m_animationClipLength.Add(EnumUIEffectType.Flash,clip.length);
						break;
				}
			}
		}
		
		#region Member

		private Dictionary<EnumUIEffectType, float> m_animationClipLength = new Dictionary<EnumUIEffectType, float>();

		private const string m_flashAnimationName = "UIEffectFlash";

		#endregion
	}
}