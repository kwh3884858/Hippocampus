using System.Collections.Generic;
using Config.Data;
using Controllers.Subsystems;
using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
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
        
        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            var cgSceneData = data as CGSceneDataProvider;
            SetSceneID(cgSceneData.CGSceneID);
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

        public void SetSceneID(string sceneID)
        {
            SceneID = sceneID;
            SceneInfo = CGSceneConfig.GetConfigByKey(sceneID);
            m_pointInfos.Clear();
            foreach (var info in SceneInfo.pointIDs)
            {
                m_pointInfos.Add(CGSceneController.GetScenePointInfo(info));
            }
        }

        public List<CGScenePointInfo> GetPointInfos()
        {
            return m_pointInfos;
        }

        
        #region Member

        public CGSceneController CGSceneController => UiDataProvider.ControllerManager.CGSceneController;

        public string SceneID;
        public CGSceneConfig SceneInfo;
        private List<CGScenePointInfo> m_pointInfos = new List<CGScenePointInfo>();
        
        #endregion
    }
}