using System.Collections.Generic;
using Config.Data;
using LocalCache;
using StarPlatinum;
using StarPlatinum.EventManager;

namespace Controllers.Subsystems
{
    public class CGScenePointInfo
    {
        public int ID
        {
            get { return ArchiveInfo.ID; }
            set { ArchiveInfo.ID = value; }
        } 
        public int touchNum {
            get { return ArchiveInfo.touchNum; }
            set { ArchiveInfo.touchNum = value; }
        }

        public int AttchID { get; private set; }
        public int MaxTouchNum { get; private set; }
        
        public CGScenePointArchiveInfo ArchiveInfo;
        public CGScenePointInfo(CGScenePointArchiveInfo info)
        {
            ArchiveInfo = info;
            SetInfo();
        }

        public CGScenePointInfo(int ID, int touchNum)
        {
            ArchiveInfo = new CGScenePointArchiveInfo(){ID = ID,touchNum = touchNum};
            SetInfo();
        }
        
        public int GetCurTouchID()
        {
            if (AttchID != 0)
            {
                return AttchID * 100 + touchNum;
            }
            return ID * 100 + touchNum;
        }

        public bool IsTouchMax()
        {
            return touchNum >= MaxTouchNum;
        }

        private void SetInfo()
        {
            var config = CGScenePointConfig.GetConfigByKey(ID);
            AttchID = config.AttachID;
            if (AttchID != 0)
            {
                config = CGScenePointConfig.GetConfigByKey(AttchID);
            }
            MaxTouchNum = config.touchConfigIDs.Count;
        }
    }

    public class CGScenePointArchiveInfo
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
            if (!m_pointInfos.ContainsKey(pointID))
            {
                var attchID = GetAttach(pointID);
                var touchNum = 0;
                if (attchID != 0)
                {
                    touchNum = GetScenePointInfo(attchID).touchNum;
                }
                m_pointInfos[pointID] = new CGScenePointInfo(pointID,touchNum);
                SetPointMaxTouchNum(pointID);
            }
            return m_pointInfos[pointID];
        }

        public bool CheckCGIsClear(string cgID)
        {
            var cfg = CGSceneConfig.GetConfigByKey(cgID);
            foreach (var pointID in cfg.pointIDs)
            {
                if (!m_pointInfos.ContainsKey(pointID)||!m_pointInfos[pointID].IsTouchMax())
                {
                    return false;
                }
            }
            return true;
        }

        public void TouchPoint(int pointID)
        {
            var info =GetScenePointInfo(pointID);

            if (!info.IsTouchMax())
            {
                info.touchNum++;
                if (info.AttchID != 0)
                {
                    RefreshAttachTouchNum(pointID,info.touchNum);
                }
                EventManager.Instance.SendEvent(new CGScenePointInfoChangeEvent());
            }
        }

        private void RefreshAttachTouchNum(int pointID,int touchNum)
        {
            var info = GetScenePointInfo(pointID);
            info.touchNum = touchNum;
            foreach (var pointInfo in m_pointInfos)
            {
                if (pointInfo.Value.AttchID == pointID)
                {
                    pointInfo.Value.touchNum = touchNum;
                }
            }
        }

        public CGScenePointTouchConfig GetTouchConfigByPointID(int pointID)
        {
            var info = GetScenePointInfo(pointID);
            int key = 0;
            if (!info.IsTouchMax())
            {
                key = info.GetCurTouchID() + 1;
            }
            else
            {
                key = info.GetCurTouchID();
            }
            return CGScenePointTouchConfig.GetConfigByKey(key);
        }
        
        private void OnPlayerPreSaveArchive(object sender, PlayerPreSaveArchiveEvent e)
        {
            //处理存储信息
            //将存储信息存入PlayerArchiveController.CurrentArchiveData中
            foreach (var pointInfo in m_pointInfos)
            {
                m_data.PointInfos[pointInfo.Key] = pointInfo.Value.ArchiveInfo;
            }
            Data.ControllerManager.PlayerArchiveController.CurrentArchiveData.CgSceneArchiveData = m_data;
        }
        
        private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
        {
            m_data = Data.ControllerManager.PlayerArchiveController.CurrentArchiveData.CgSceneArchiveData;
            m_pointMaxTouchNums.Clear();
            if (m_data.PointInfos == null)
            {
                m_data.PointInfos = new Dictionary<int, CGScenePointArchiveInfo>();
            }
            m_pointInfos.Clear();
            foreach (var pointInfo in m_data.PointInfos)
            {
                m_pointInfos.Add(pointInfo.Key,new CGScenePointInfo(pointInfo.Value));
                SetPointMaxTouchNum(pointInfo.Key);
            }
        }

        private int GetAttach(int pointID)
        {
            var pointCfg = CGScenePointConfig.GetConfigByKey(pointID);
            return pointCfg.AttachID;
        }

        private void SetPointMaxTouchNum(int pointID)
        {
            var pointCfg = CGScenePointConfig.GetConfigByKey(pointID);
            if (pointCfg.AttachID != 0)
            {
                return;
            }
            m_pointMaxTouchNums[pointID] = pointCfg.touchConfigIDs.Count;
        }

        private CGSceneArchiveData m_data;
        private Dictionary<int,CGScenePointInfo> m_pointInfos = new Dictionary<int,CGScenePointInfo>();
        private Dictionary<int,int> m_pointMaxTouchNums = new Dictionary<int, int>();
    }
}