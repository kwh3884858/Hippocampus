using System.Collections.Generic;
using Config.Data;
using LocalCache;
using StarPlatinum;
using StarPlatinum.EventManager;

namespace Controllers.Subsystems
{
    public class CGScenePointInfo
    {
        public int ID;
        public int touchNum;

        public int GetCurTouchID()
        {
            return ID * 100 + touchNum;
        }
    }

    public class CGScenePointData
    {
        public int maxTouchInfo;
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
            if (!m_data.PointInfos.ContainsKey(pointID))
            {
                SetPointMaxTouchNum(pointID);
                m_data.PointInfos[pointID] = new CGScenePointInfo(){ID = pointID,touchNum = 0};
            }
            return m_data.PointInfos[pointID];
        }

        public bool CheckCGIsClear(string cgID)
        {
            var cfg = CGSceneConfig.GetConfigByKey(cgID);
            foreach (var pointID in cfg.pointIDs)
            {
                if (!m_data.PointInfos.ContainsKey(pointID)||m_data.PointInfos[pointID].touchNum < m_pointMaxTouchNums[pointID])
                {
                    return false;
                }
            }
            return true;
        }

        public string TouchPointAndGetStoryID(int pointID)
        {
            if (m_data.PointInfos.ContainsKey(pointID))
            {
                if (m_data.PointInfos[pointID].touchNum < m_pointMaxTouchNums[pointID])
                {
                    m_data.PointInfos[pointID].touchNum++;
                    EventManager.Instance.SendEvent(new CGScenePointInfoChangeEvent());
                }
            }
            else
            {
                m_data.PointInfos[pointID]=new CGScenePointInfo(){ID = pointID,touchNum =1};
                SetPointMaxTouchNum(pointID);
                EventManager.Instance.SendEvent(new CGScenePointInfoChangeEvent());
            }

            var touchCfg = CGScenePointTouchConfig.GetConfigByKey(m_data.PointInfos[pointID].GetCurTouchID());
            return touchCfg.storyID;
        }

        public CGScenePointTouchConfig GetTouchConfigByPointID(int pointID)
        {
            int key = pointID*100+1;
            if (m_data.PointInfos.ContainsKey(pointID)&&m_pointMaxTouchNums[pointID]>m_data.PointInfos[pointID].touchNum)
            {
                key = m_data.PointInfos[pointID].GetCurTouchID() + 1;
            }
            else if(m_data.PointInfos.ContainsKey(pointID))
            {
                key = m_data.PointInfos[pointID].GetCurTouchID();
            }
            return CGScenePointTouchConfig.GetConfigByKey(key);
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
            m_pointMaxTouchNums.Clear();
            if (m_data.PointInfos == null)
            {
                m_data.PointInfos = new Dictionary<int, CGScenePointInfo>();
            }
            foreach (var pointInfo in m_data.PointInfos)
            {
                SetPointMaxTouchNum(pointInfo.Key);
            }
        }

        private void SetPointMaxTouchNum(int pointID)
        {
            var pointCfg = CGScenePointConfig.GetConfigByKey(pointID);
            m_pointMaxTouchNums[pointID] = pointCfg.touchConfigIDs.Count;
        }

        private CGSceneArchiveData m_data;
        private Dictionary<int,int> m_pointMaxTouchNums = new Dictionary<int, int>();
    }
}