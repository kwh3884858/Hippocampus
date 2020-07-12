using System.Collections.Generic;
using Config.Data;
using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
    public class PointInfo
    {
        public int ID;
        public int touchNum;
    }
    public class CommonCGSceneModel: UIModel
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

        private void SetSceneID(string sceneID)
        {
            m_sceneID = sceneID;
            m_sceneInfo = CGSceneConfig.GetConfigByKey(sceneID);
            m_pointInfos.Clear();
            foreach (var info in m_sceneInfo.pointIDs)
            {
                m_pointInfos.Add(new PointInfo(){ID = info,touchNum = 0});
            }
        }

        private List<PointInfo> GetPointInfos()
        {
            return m_pointInfos;
        }

        
        #region Member

        public string m_sceneID;
        private CGSceneConfig m_sceneInfo;
        private List<PointInfo> m_pointInfos = new List<PointInfo>();
        
        #endregion
    }
}