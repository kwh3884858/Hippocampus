using System.Collections.Generic;
using Controllers.Subsystems;
using UI.Panels.Providers.DataProviders;
using UnityEngine;

namespace UI.Panels
{
    public class CommonLogModel: UIModel
    {
        #region template method
        public override void Initialize(IUiPanel uiPanel )
        {
            base.Initialize(uiPanel);
        }

        public override void DeInitialize()
        {
            base.DeInitialize();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
        }

        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            RefreshLogInfos();
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void LateTick()
        {
            base.LateTick();
        }

        public override void SubpanelChanged(UIPanelType type, DataProvider data = null)
        {
            base.SubpanelChanged(type,data);
        }

        public override void SubpanelDataChanged(UIPanelType type, DataProvider data)
        {
            base.SubpanelDataChanged(type,data);
        }
        #endregion

        public int GetLogNum()
        {
            if (m_logInfo == null)
            {
                return 0;
            }
            return m_logInfo.Count;
        }

        public LogInfo GetLogByIndex(int index)
        {
            if (m_logInfo.Count <= index)
            {
                Debug.LogError("历史记录获取错误!!!");
            }
            return m_logInfo[index];
        }
        
        private void RefreshLogInfos()
        {
            m_logInfo = UiDataProvider.Data.ControllerManager.LogController.GetLogInfos();
            m_logInfo.Reverse();
        }
        
        #region Member

        private List<LogInfo> m_logInfo;

        #endregion
    }
}