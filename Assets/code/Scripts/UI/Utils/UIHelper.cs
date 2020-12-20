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

        public static bool IsCursorLocked()
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }
        
        public static void LockCursor(bool locked)
        {
            if (locked)
            {
                VMCameraManager.Instance().SetCameraRotate(true);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                VMCameraManager.Instance().SetCameraRotate(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        private static List<UIPanelType> m_unlockCursorPanelList= new List<UIPanelType>();
        public static void AddUnlockPanel(UIPanelType panelType)
        {
            if (!m_unlockCursorPanelList.Contains(panelType))
            {
                m_unlockCursorPanelList.Add(panelType);
                if (m_unlockCursorPanelList.Count > 0 && IsCursorLocked())
                {
                    LockCursor(false);
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
                }
            }
        }
    }
}