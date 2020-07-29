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
        }

        public void PushSceneID(string sceneID)
        {
            if (!string.IsNullOrEmpty(SceneID))
            {
                m_sceneStack.Push(SceneID);
            }
            SetSceneID(sceneID);
        }

        public List<CGScenePointInfo> GetPointInfos()
        {
            List<CGScenePointInfo> pointInfos = new List<CGScenePointInfo>();

            foreach (var info in SceneInfo.pointIDs)
            {
                pointInfos.Add(CGSceneController.GetScenePointInfo(info));
            }
            return pointInfos;
        }

        public bool PopScene()
        {
            if (m_sceneStack.Count<=0)
            {
                return false;
            }
            SetSceneID(m_sceneStack.Pop());
            return true;
        }

        
        #region Member

        public CGSceneController CGSceneController => UiDataProvider.ControllerManager.CGSceneController;

        public string SceneID;
        public CGSceneConfig SceneInfo;

        private Stack<string> m_sceneStack=new Stack<string>();
        #endregion
    }
}