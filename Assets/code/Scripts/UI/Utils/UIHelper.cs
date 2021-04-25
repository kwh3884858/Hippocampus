using StarPlatinum.EventManager;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Utils
{
    public class UIHelper
    {
        public static string GetHMCounterDown(long remainTime)
        {
            if (remainTime < 0)
            {
                Debug.LogWarning("倒计时小于0");
                return null;
            }

            var hour = remainTime / 3600;
            var min = remainTime % 3600 / 60;
            
            return hour.ToString("00")+":"+min.ToString("00");
        }

        private static float GetAnimationTime(Animator animator, string animationName)
        {
            float length = 0;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach(AnimationClip clip in clips)
            {
                if(clip.name.Equals(animationName))
                {
                    length = clip.length;
                    break;
                }
            }

            return length;
        }

        public static bool IsCursorLocked()
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }
        
        public static void LockCursor(bool locked)
        {
            return;
            if (locked)
            {
                VMCameraManager.Instance().SetCameraRotate(true);
                //Hack!!!
                //TODO: Please change to comply with the existing UI code style.
                //Using event callback to handle cursor event.
                if (GamePlay.Global.SingletonGlobalDataContainer.Instance.PlatformCtrl.IsMobile)
                {
                    UI.UIManager.Instance().ShowPanel(UIPanelType.JoystickPanel);// 显示摇杆UI
                    UI.UIManager.Instance().ShowPanel(UIPanelType.UIExplorationCameraviewpointPanel);// 显示摄像机控制UI
                }

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                VMCameraManager.Instance().SetCameraRotate(false);
                //Hack!!!
                //TODO: Please change to comply with the existing UI code style.
                //Using event callback to handle cursor event.
                if (GamePlay.Global.SingletonGlobalDataContainer.Instance.PlatformCtrl.IsMobile)
                {
                    UI.UIManager.Instance().HidePanel(UIPanelType.JoystickPanel);// 关闭摇杆UI
                    UI.UIManager.Instance().HidePanel(UIPanelType.UIExplorationCameraviewpointPanel);// 关闭摄像机控制UI
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        private static List<UIPanelType> m_unlockCursorPanelList= new List<UIPanelType>();
        
        public class CursorEvent : System.EventArgs
        {
            public bool m_isCameraCanMove;
        }
        public static void AddUnlockPanel(UIPanelType panelType)
        {
            if (!m_unlockCursorPanelList.Contains(panelType))
            {
                m_unlockCursorPanelList.Add(panelType);
                if (m_unlockCursorPanelList.Count > 0 && IsCursorLocked())
                {
                    LockCursor(false);
                    EventManager.Instance.SendEvent(new CursorEvent { m_isCameraCanMove = true});
                    return;
                }
            }
        }

        public static void RemoveUnlockPanel(UIPanelType panelType)
        {
            if (m_unlockCursorPanelList.Contains(panelType))
            {
                m_unlockCursorPanelList.Remove(panelType);
                if (m_unlockCursorPanelList.Count == 0 && !IsCursorLocked())
                {
                    LockCursor(true);
                    EventManager.Instance.SendEvent(new CursorEvent { m_isCameraCanMove = false });
                }
            }
        }
    }
}