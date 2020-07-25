using System.Collections.Generic;
using LocalCache;
using StarPlatinum;
using StarPlatinum.EventManager;

namespace Controllers.Subsystems
{
    public class CGScenePointInfo
    {
        public int ID;
        public int touchNum;
    }
    public class CGSceneController: ControllerBase
    {
        public override void Initialize(IControllerProvider args)
        {
            base.Initialize(args);
            EventManager.Instance.AddEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);

        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void Terminate()
        {
            base.Terminate();
            EventManager.Instance.RemoveEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);

        }

        public CGScenePointInfo GetScenePointInfo(int pointID)
        {
            return m_data.PointInfos[pointID];
        }
        
        private void OnPlayerPreSaveArchive(object sender, PlayerPreSaveArchiveEvent e)
        {
            //处理存储信息
            //将存储信息存入PlayerArchiveController.CurrentArchiveData中
            Data.ControllerManager.PlayerArchiveController.CurrentArchiveData.CgSceneArchiveData = m_data;
        }
        
        private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
        {
            m_data = Data.ControllerManager.PlayerArchiveController.CurrentArchiveData.CgSceneArchiveData;
        }

        private CGSceneArchiveData m_data;

    }
}