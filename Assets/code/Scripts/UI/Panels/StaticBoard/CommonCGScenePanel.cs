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
			
			m_btn_exit_Button.onClick.AddListener(() =>
			{
				Application.Quit();
			});
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

			CloseAllCGItems();
			if (m_cgItems.ContainsKey(config.CGItemKey))
			{
				RefreshCGSceneInfo();
			}
			else
			{
				PrefabManager.Instance.InstantiateAsync<UI_Common_CGscene_Item_SubView>(config.CGItemKey, (result) =>
				{
					if (result.status != RequestStatus.SUCCESS)
					{
						return;
					}

					var item = result.result as UI_Common_CGscene_Item_SubView;
					m_cgItems.Add(config.CGItemKey, item);
					item.Init(item.GetComponent<RectTransform>());
					if (m_model.SceneInfo.CGItemKey != config.CGItemKey)
					{
						item.Hide();
						return;
					}
					RefreshCGSceneInfo();
				},m_go_CGSceneItems);
				return;
			}
		}

		private void CloseAllCGItems()
		{
			foreach (var cgItem in m_cgItems)
			{
				cgItem.Value.Hide();
			}
		}

		private void RefreshCGSceneInfo()
		{
			m_cgItems[m_model.SceneInfo.CGItemKey].SetInfo(m_model.SceneInfo);
			RefreshPointInfos();
		}

		private void RefreshPointInfos()
		{
			m_btn_back_Button.gameObject.SetActive(CgSceneController.CheckCGIsClear(m_model.SceneID));
			m_cgItems[m_model.SceneInfo.CGItemKey].RefreshPointInfos(m_model.GetPointInfos(),OnClickPoint);
		}

		private void OnCGScenePointInfoChange(object sender, CGScenePointInfoChangeEvent e)
		{
			if (CgSceneController.CheckCGIsClear(m_model.SceneID)&&m_model.SceneID == "EP04-01")
			{
				m_btn_exit_Button.gameObject.SetActive(true);
				return;
			}
			RefreshPointInfos();
		}
		

		private void OnClickPoint(int pointID,CGScenePointTouchConfig config)
		{
			CgSceneController.TouchPoint(pointID);
			if (config.touchType == (int)CGScenePointTouchType.DeathBody)
			{
				m_model.PushSceneID(config.Parameter);
				RefreshInfo();
				return;
			}
			InvokeShowPanel(UIPanelType.TalkPanel,new TalkDataProvider(){ID = config.Parameter});
		}

		#region Member

		public CGSceneController CgSceneController => UiDataProvider.ControllerManager.CGSceneController;
		private Dictionary<string, UI_Common_CGscene_Item_SubView> m_cgItems = new Dictionary<string, UI_Common_CGscene_Item_SubView>();

		#endregion
	}
}