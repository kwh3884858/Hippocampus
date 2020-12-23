using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Player;
using StarPlatinum.Base;
using StarPlatinum.Services;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace Gameplay.Manager
{
    [System.Serializable]
    public enum TimelineEnum
    {
        Default,
        StartScene,
    };
    public class TimelineManager : MonoSingleton<TimelineManager>
    {


        //Deprecated
        Dictionary<TimelineEnum, TimelineAsset> m_timelineAssetDic = new Dictionary<TimelineEnum, TimelineAsset>(); 
        public TimelineAsset[] m_assets;//实现在Inspector面板的显示

        public override void SingletonInit()
        {
            m_TimelinePlayer = Object.FindObjectOfType<PlayableDirector>();
            //m_timelineAssetDic.Add(TimelineEnum.Default, m_assets[0] != null ? m_assets[0] : null);
            //m_timelineAssetDic.Add(TimelineEnum.StartScene, m_assets[1] != null ? m_assets[1] : null);
        }


        public void PlayTimeline(TimelineEnum TimelineType,Transform PlayerPosition = null)
        {
            Debug.Log("Play Timeline");
            m_timelineAssetDic.TryGetValue(TimelineType, out var timelineAsset);
            if (timelineAsset && m_TimelinePlayer)
            {
                PlayTimeline(timelineAsset);
            }
        }

        public void PlayTimeline(TimelineAsset timelineAsset, Transform PlayerPosition = null)
        {
            if (!m_TimelinePlayer)
            {
                return;
            }

            if (m_TimelinePlayer.state == PlayState.Playing)
            {
                return;
            }

            m_TimelinePlayer.Play(timelineAsset);
            Debug.Log("PlayableDirector named " + m_TimelinePlayer.name + " is now playing in " + m_TimelinePlayer.time + " left time is " + (m_TimelinePlayer.duration - m_TimelinePlayer.time));

            EnterCinemaState();

            //callback
            StartCoroutine(PlayedCallBack(m_TimelinePlayer.duration - m_TimelinePlayer.time, PlayerPosition));
        }

        IEnumerator PlayedCallBack(double time, Transform playerPosition)
        {
            if (playerPosition != null)
            {
                PlayerController.Instance().SetPlayerPosition(playerPosition);
            }
            yield return new WaitForSeconds((float)time);

            if (m_shouldAbortCurrentTimeline)
            {
                m_shouldAbortCurrentTimeline = false;
                yield break;
            }

            ExitCinemaState();

            yield break;
        }

        public void AbortTimeline()
        {
            if (m_TimelinePlayer.state == PlayState.Playing)
            {
                m_TimelinePlayer.Stop();
                m_TimelinePlayer.initialTime = m_TimelinePlayer.duration;
                m_TimelinePlayer.Play();
                ExitCinemaState();
                m_shouldAbortCurrentTimeline = true;
            }
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(0, 100, 200, 50), "Skip Timeline(F4)")||  UnityEngine.InputSystem.Keyboard.current.f4Key.wasPressedThisFrame)
            {
                Debug.Log("skip timeline");
                Gameplay.Manager.TimelineManager.Instance().AbortTimeline();
            }
        }

        private void EnterCinemaState()
        {
            //FP
            PlayerController.Instance().SetMoveEnable(false);
            CameraService.Instance.SwitchRaycastState(false);
            UI.UIManager.Instance().HideStaticPanel(UIPanelType.UICommonGameplayPanel);
            UI.UIManager.Instance().HideStaticPanel(UIPanelType.UICommonBookmarkPanel);
            if (GamePlay.Global.SingletonGlobalDataContainer.Instance.PlatformCtrl.IsMobile)
            {
                UI.UIManager.Instance().HidePanel(UIPanelType.JoystickPanel);// 关闭摇杆UI
                UI.UIManager.Instance().HidePanel(UIPanelType.UIExplorationCameraviewpointPanel);// 关闭摄像机控制UI
            }
        }

        private void ExitCinemaState()
        {
            //FP
            PlayerController.Instance().SetMoveEnable(true);
            CameraService.Instance.SwitchRaycastState(true);
            UI.UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonGameplayPanel);
            UI.UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonBookmarkPanel);
            if (GamePlay.Global.SingletonGlobalDataContainer.Instance.PlatformCtrl.IsMobile)
            {
                UI.UIManager.Instance().ShowPanel(UIPanelType.JoystickPanel);// 显示摇杆UI
                UI.UIManager.Instance().ShowPanel(UIPanelType.UIExplorationCameraviewpointPanel);// 显示摄像机控制UI
            }
        }

        private PlayableDirector m_TimelinePlayer;
        private bool m_shouldAbortCurrentTimeline = false;
    }
}
