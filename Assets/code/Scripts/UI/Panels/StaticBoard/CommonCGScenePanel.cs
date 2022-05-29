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
using System;
using Evidence;

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
			m_btn_back_Button.onClick.AddListener(CloseCGScene);
			m_btn_showImg_Button.onClick.AddListener(OnClickShowImg);

			EventManager.Instance.AddEventListener<CGScenePointInfoChangeEvent>(OnCGScenePointInfoChange);
			EventManager.Instance.AddEventListener<CGSceneCloseEvent>(OnCGSceneClose);
            EventManager.Instance.AddEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.AddEventListener(EventKey.EventStoryTrigger,OnEvidenceStoryTrigger);
	

        }

        private void OnPlayerPreSaveArchive(object sender, PlayerPreSaveArchiveEvent e)
        {
			GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.CgSceneArchiveData.CgSceneId = m_model.SceneID;
		}

        public override void DeInitialize()
		{
			m_model.Deactivate();
			base.DeInitialize();
			EventManager.Instance.RemoveEventListener<CGScenePointInfoChangeEvent>(OnCGScenePointInfoChange);
            EventManager.Instance.RemoveEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.RemoveEventListener<CGSceneCloseEvent>(OnCGSceneClose);
            EventManager.Instance.RemoveEventListener(EventKey.EventStoryTrigger,OnEvidenceStoryTrigger);

        }

        public override void Hide()
		{
			m_model.Hide();
			base.Hide();
			EventManager.Instance.SendEvent(new ChangeCursorEvent());
            GamePlay.Player.PlayerController.Instance().SetMoveEnable(true);
        }

        public override void Deactivate()
		{
			m_model.Deactivate();
			base.Deactivate();
		}

		public override void ShowData(DataProvider data)
		{
			m_model.ShowData(data);
            GamePlay.Player.PlayerController.Instance().SetMoveEnable(false);
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
			GetTriggeredEvidenceStory();
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
		
		private void OnCGSceneClose(object sender, CGSceneCloseEvent e)
        {
			string cgSceneId = e.m_cgSceneId;
            if (cgSceneId == "")
            {
				Debug.LogWarning("Want to closed scene name is empty");
				CloseCGScene();
				return;
            }
            if (cgSceneId != null && cgSceneId != m_model.SceneID )
            {
				Debug.LogError("Current scene is: " + m_model.SceneID + "\n But you ask for " + cgSceneId);
				return;
            }
			CloseCGScene();
		}

        private void CloseCGScene()
        {
            var config = m_model.SceneInfo;

            string sceneName = null;
            string missionName = null;
            string showLabel = null;
            
            if (m_storyInfo != null)
            {
	            if (!m_isShowStory && !string.IsNullOrEmpty(m_storyInfo.config.StoryId))
	            {
		            m_isShowStory = true;
		            InvokeShowPanel(UIPanelType.UICommonTalkPanel,
			            new TalkDataProvider() {ID = m_storyInfo.config.StoryId, OnTalkEnd = CloseCGScene});
		            return;
	            }

	            sceneName = m_storyInfo.config.LoadSceneNameOnEnd;
	            missionName = m_storyInfo.config.LoadMissionIDOnEnd;
	            EvidenceDataManager.Instance.RemoveEvidenceStory(m_storyInfo);
	            m_storyInfo = null;
            }

            if (!string.IsNullOrEmpty(config.LoadSceneNameOnEnd))
            {
	            sceneName = config.LoadSceneNameOnEnd;
            }
            if (!string.IsNullOrEmpty(config.LoadMissionIDOnEnd))
            {
	            missionName = config.LoadMissionIDOnEnd;
            }

            if (!string.IsNullOrEmpty(config.ShowLabelOnEnd))
            {
	            showLabel = config.ShowLabelOnEnd;
            }

            if (!string.IsNullOrEmpty(sceneName))
            {
                GameSceneManager.Instance.LoadScene(SceneLookup.GetEnum(sceneName, false));
            }
            
            if (!string.IsNullOrEmpty(config.ShowLabelOnEndStoryFileName))
            {
	            UiDataProvider.ControllerManager.StoryController.LoadStoryFileByName(config.ShowLabelOnEndStoryFileName);
            }
            if (!string.IsNullOrEmpty(showLabel))
            {
	            InvokeShowPanel(UIPanelType.UICommonTalkPanel,
		            new TalkDataProvider() {ID = showLabel});
            }

            if (!string.IsNullOrEmpty(missionName))
            {
                MissionSceneManager.Instance.LoadMissionScene(MissionSceneManager.Instance.GetMissionEnumBy(missionName, false));
                InvokeHidePanel();
                return;
            }

            if (m_model.PopScene())
            {
                RefreshInfo();
            }
            else
            {
                InvokeHidePanel();
            }
        }

        private void ClickPointAction(CGScenePointTouchConfig config)
        {
	        if (config.touchType == (int)CGScenePointTouchType.DeathBody)
	        {
		        m_model.PushSceneID(config.Parameter);
		        RefreshInfo();
		        return;
	        }
	        InvokeShowPanel(UIPanelType.UICommonTalkPanel,new TalkDataProvider(){ID = config.Parameter});
        }

        private CGScenePointTouchConfig m_curClickPointConfig;
        private void OnClickPoint(int pointID,CGScenePointTouchConfig config)
		{
			SoundService.Instance.PlayEffect(CommonConfig.Data.CGSceneClickSoundEffect);
			CgSceneController.TouchPoint(pointID);
			if (!string.IsNullOrEmpty(config.showImgKey))
			{
				EventManager.Instance.SendEvent(EventKey.ShowUIEffect,new UIEffectData(){ Type = EnumUIEffectType.Flash, EndCallback =
					() =>
					{
						m_btn_showImg_Button.gameObject.SetActive(true);
						PrefabManager.Instance.SetImage(m_img_showImg_Image,config.showImgKey);
						m_curClickPointConfig = config;
					}});
			}
			else
			{
				ClickPointAction(config);
			}
		}

        private void OnClickShowImg()
        {
	        m_btn_showImg_Button.gameObject.SetActive(false);
	        ClickPointAction(m_curClickPointConfig);
        }

        #region 证物剧情触发

        private void OnEvidenceStoryTrigger(object data)
        {
	        EvidenceStoryInfo info = data as EvidenceStoryInfo;
	        AddEvidenceStory(info);
        }

        private void GetTriggeredEvidenceStory()
        {
	        AddEvidenceStory(EvidenceDataManager.Instance.GetTriggeredEvidenceStory());
        }

        private void AddEvidenceStory(EvidenceStoryInfo info)
        {
	        m_storyInfo = info;
	        m_isShowStory = false;
        }

        #endregion

		#region Member

		public CGSceneController CgSceneController => UiDataProvider.ControllerManager.CGSceneController;
		private Dictionary<string, UI_Common_CGscene_Item_SubView> m_cgItems = new Dictionary<string, UI_Common_CGscene_Item_SubView>();

		private EvidenceStoryInfo m_storyInfo;
		private bool m_isShowStory = false;

		#endregion
	}
}