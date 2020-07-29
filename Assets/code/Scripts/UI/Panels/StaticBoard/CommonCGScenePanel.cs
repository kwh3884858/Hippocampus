using System.Collections;
using System.Collections.Generic;
using Config.Data;
using Config.GameRoot;
using Controllers.Subsystems;
using DG.Tweening;
using GamePlay.Stage;
using StarPlatinum;
using StarPlatinum.EventManager;
using UI.Panels.Element;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.StaticBoard;

namespace UI.Panels
{
	public partial class CommonCGScenePanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);	
			
			m_btn_back_Button.onClick.AddListener(() =>
			{
				var config = m_model.SceneInfo;
				if (!string.IsNullOrEmpty(config.LoadSceneNameOnEnd))
				{
					GameSceneManager.Instance.LoadScene(SceneLookup.GetEnum(config.LoadSceneNameOnEnd,false));
				}

				if (!string.IsNullOrEmpty(config.LoadMissionIDOnEnd))
				{
					MissionSceneManager.Instance.LoadMissionScene(MissionSceneManager.Instance.GetMissionEnumBy(config.LoadMissionIDOnEnd,false));
				}

				if (m_model.PopScene())
				{
					RefreshInfo();
				}
				else
				{
					InvokeHidePanel();
				}
			});
			EventManager.Instance.AddEventListener<CGScenePointInfoChangeEvent>(OnCGScenePointInfoChange);
		}

		public override void DeInitialize()
		{
			m_model.Deactivate();
			base.DeInitialize();
			EventManager.Instance.RemoveEventListener<CGScenePointInfoChangeEvent>(OnCGScenePointInfoChange);
		}

		public override void Hide()
		{
			m_model.Hide();
			base.Hide();
			EventManager.Instance.SendEvent(new ChangeCursorEvent());
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
			RefreshInfo();
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

		private void RefreshInfo()
		{
			var config = m_model.SceneInfo;
			if (!string.IsNullOrEmpty(config.StoryFileName))
			{
				UiDataProvider.ControllerManager.StoryController.LoadStoryFileByName(config.StoryFileName);
			}
			PrefabManager.Instance.SetImage(m_img_cg_Image,config.CGKey);
			RefreshPointInfos();
		}

		private void RefreshPointInfos()
		{
			EventManager.Instance.SendEvent(new ChangeCursorEvent());
			m_btn_back_Button.gameObject.SetActive(CgSceneController.CheckCGIsClear(m_model.SceneID));
			ClearPointItem();
			var pointInfos = m_model.GetPointInfos();
			for (int i = 0; i < pointInfos.Count; i++)
			{
				var pointInfo = pointInfos[i];
				if (m_items.Count <= i)
				{
					PrefabManager.Instance.InstantiateAsync<CGScenePointItem>("UI_Common_CGScenePoint_IItem", (result) =>
					{
						if (result.status != RequestStatus.SUCCESS)
						{
							return;
						}

						var item = result.result as CGScenePointItem;
						m_items.Add(item);
						item.ClickCallback += OnClickPoint;
						item.SetPointInfo(pointInfo);
					},m_go_interactionPoints);
					continue;
				}
				m_items[i].SetPointInfo(pointInfo);
				m_items[i].gameObject.SetActive(true);
			}
		}

		private void OnCGScenePointInfoChange(object sender, CGScenePointInfoChangeEvent e)
		{
			RefreshPointInfos();
		}

		private void ClearPointItem()
		{
			foreach (var pointItem in m_items)
			{
				pointItem.gameObject.SetActive(false);
			}
		}

		private void OnClickPoint(int pointID,CGScenePointTouchConfig config)
		{
			if (config.touchType == (int)CGScenePointTouchType.DeathBody)
			{
				m_model.PushSceneID(config.Parameter);
				RefreshInfo();
				return;
			}
			CgSceneController.TouchPoint(pointID);
			InvokeShowPanel(UIPanelType.TalkPanel,new TalkDataProvider(){ID = config.Parameter});
		}

		#region Member

		public CGSceneController CgSceneController => UiDataProvider.ControllerManager.CGSceneController;
		private List<CGScenePointItem> m_items = new List<CGScenePointItem>();

		#endregion
	}
}