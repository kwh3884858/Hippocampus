﻿using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Const;
using Controllers.Subsystems.Story;
using GamePlay.Player;
using GamePlay.Stage;
using Spine;
using StarPlatinum;
using StarPlatinum.EventManager;
using TMPro;
using UI.Panels.Element;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;
using UnityEngine.UI;
using UI.UIComponent;

namespace UI.Panels
{
    public class MGStartManuMainManuPanel : UIPanel<UIDataProvider, DataProvider>
    {

        [SerializeField]
        private bool isShowLogo;
        [SerializeField]
        private UnityEngine.Animation anim = null;
        /// <summary>logo显示的整体</summary>
        [SerializeField]
        private LogoAnimCtrl logoAnimCtrl = null;

        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
            EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
            if (isShowLogo)
            {
                if(anim != null)
                {
                    anim.playAutomatically = false;
                }
                if(logoAnimCtrl != null)
                {
                    logoAnimCtrl.Show(()=> {
                        if (anim != null)
                        {
                            anim.Play();
                        }
                    });
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
            EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);

        }

        public void ShowHud()
        {
            //			UiDataProvider.StaticBoard.ShowTalk ("9", OnTalkEnd);
            //GamePlay.Player.PlayerController.Instance().SetMoveEnable(true);
            //if (GamePlay.Global.SingletonGlobalDataContainer.Instance.PlatformCtrl.IsMobile)
            //{
            //    UIManager.Instance().ShowPanel(UIPanelType.JoystickPanel);// 显示摇杆UI
            //    UIManager.Instance().ShowPanel(UIPanelType.UIExplorationCameraviewpointPanel);// 显示摄像机控制UI
            //}
        }

        public void HidSelef()
        {
            base.InvokeHidePanel();
            // 测试
            //this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 点击开始游戏按钮，手动绑定
        /// </summary>
        public void OnClickStartBtn()
        {

            //Now we don`t load scene in UI button, we use mission to manager state, and input system UI like HUD should display when player is controllable.

            //Read start scene and mission from archive file
            //GameSceneManager.Instance.LoadScene(ConfigRoot.Instance.StartScene, "", delegate()
            //{
            //    MissionSceneManager.Instance.LoadMissionScene(ConfigRoot.Instance.StartMission);
            //});

            CallbackTime(1, ShowHud);
            UiDataProvider.Data.Data.ChangeGameState(GameState.Battle);
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonGameplayPanel);// 显示助手UI
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonBookmarkPanel);// 显示书签

            #region Timeline Enable(Obsolete)
            //TimelineManager.Instance().PlayTimeline(TimelineEnum.StartScene);
            //SkyboxManager.Instance().UpdateSkybox(SkyboxEnum.Sunday);
            #endregion

            //StarPlatinum.PrefabManager.Instance.LoadScene (SceneLookupEnum.World_Episode2_Pier, UnityEngine.SceneManagement.LoadSceneMode.Additive);

            //未读取存档
            if (UiDataProvider.ControllerManager.PlayerArchiveController.CurrentSaveIndex == -1)
            {
                UiDataProvider.ControllerManager.PlayerArchiveController.LoadData(0);
                return;
            }

            HidSelef();
        }

        /// <summary>
        /// 点击载入游戏按钮
        /// </summary>
        public void OnClickLoadBtn()
        {
            //			UIManager.Instance ().ShowPanel (UIPanelType.LoadGamePanel);
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonLoadarchivePanel, new ArchiveDataProvider() { Type = ArchivePanelType.Load });
        }

        /// <summary>
        /// 点击游戏设置按钮
        /// </summary>
        public void OnClickSettingBtn()
        {
        }

        /// <summary>
        /// 点击制作人员列表按钮
        /// </summary>
        public void OnClickProductionStaffBtn()
        {

        }

        /// <summary>
        /// 点击退出游戏按钮
        /// </summary>
        public void OnClickExitBtn()
        {
            Application.Quit();// 退出游戏
        }

        private void OnTalkEnd()
        {
            GamePlay.Player.PlayerController.Instance().SetMoveEnable(true);
        }

        private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
        {
            OnClickStartBtn();
        }
    }
}