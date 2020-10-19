using System;
using System.Collections.Generic;
using GamePlay.Stage;
using LocalCache;
using StarPlatinum;
using StarPlatinum.EventManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.Subsystems.Role
{
    public class PlayerArchiveController: ControllerBase
    {
        public override void Initialize (IControllerProvider args)
        {
            base.Initialize (args);
            ArchivePreviewData = Data.LocalCacheManager.GetData<PlayerArchivePreviewData>();
            if (ArchivePreviewData == null)
            {
                ArchivePreviewData = new PlayerArchivePreviewData();
            }

            if (ArchivePreviewData.ArchivePreviewData == null)
            {
                ArchivePreviewData.ArchivePreviewData=new List<ArchivePreviewData>();
            }
            SortArchivePreViewData();
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        public override void Tick()
        {
            base.Tick();
            m_playTime += Time.deltaTime;
            if (m_lastSaveInterval <= 1)
            {
                m_lastSaveInterval += Time.deltaTime;
            }
        }
        
        public void LoadData(int selectIndex)
        {
            //处理新存档
            if (selectIndex == 0)
            {
                CurrentArchiveData = new PlayerArchiveData();
                CurrentArchiveData.MissionArchieData= new MissionArchiveData();
                CurrentArchiveData.CgSceneArchiveData = new CGSceneArchiveData();
                CurrentArchiveData.EvidenceArchiveData = new EvidenceArchiveData();
                CurrentArchiveData.TipsArchiveData = new TipsArchiveData();
                m_playTime = 0;
                CurrentSaveIndex = 0;
                m_localCacheManager = null;
                EventManager.Instance.SendEvent(new PlayerLoadArchiveEvent());
                return;
            }
            if (m_localCacheManager != null)
            {
                m_localCacheManager.Clear();
            }
            if (ArchivePreviewData.ArchivePreviewData.Count < selectIndex)
            {
                Debug.LogError($"读档错误!!!!未知档:{selectIndex}");
                return;
            }

            var previewData = ArchivePreviewData.ArchivePreviewData[selectIndex - 1];
            m_playTime = previewData.TotalPlayTime;

            CurrentSaveIndex = selectIndex;
            m_localCacheManager = new LocalCacheManager(GetArchiveName(previewData));
            CurrentArchiveData = m_localCacheManager.GetData<PlayerArchiveData>();
            
            EventManager.Instance.SendEvent(new PlayerLoadArchiveEvent());
        }

        public void SaveData(int saveIndex)
        {
            if (saveIndex >= 3)
            {
                return;
            }
            if (m_lastSaveInterval <= 1)
            {
                return;
            }
            m_lastSaveInterval = 0;
            bool isNewData = false;
            if (ArchivePreviewData.ArchivePreviewData.Count <= saveIndex)
            {
                isNewData = true;
                ArchivePreviewData.ArchivePreviewData.Add(new ArchivePreviewData());
            }

            var previewData = ArchivePreviewData.ArchivePreviewData[ArchivePreviewData.ArchivePreviewData.Count - 1];
            if (!isNewData)
            {
                PersistentStorage.Delete(GetArchiveName(previewData));
            }
            //TODO:写入存档预览信息
            previewData.SaveTime = DateTime.Now.Ticks;
            previewData.Img = "PreviewImg"+saveIndex.ToString();
            previewData.EPName = MissionSceneManager.Instance.GetCurrentMissionEnum ().ToString ();
            previewData.TotalPlayTime = (int)m_playTime;
            SortArchivePreViewData();
            Data.LocalCacheManager.SetData(ArchivePreviewData);
            m_localCacheManager = new LocalCacheManager(previewData.SaveTime.ToString());
            EventManager.Instance.SendEvent(new PlayerPreSaveArchiveEvent());
            m_localCacheManager.SetData(CurrentArchiveData);
            EventManager.Instance.SendEvent(new PlayerSaveArchiveEvent());

        }

        private void SortArchivePreViewData()
        {
            ArchivePreviewData.ArchivePreviewData.Sort((d1, d2) => { return d2.SaveTime.CompareTo(d1.SaveTime);});
        }
        
        private string GetArchiveName(ArchivePreviewData data)
        {
            if (data == null)
            {
                return "newOne";
            }
            return data.SaveTime.ToString();
        }

        //当前存档数据
        public PlayerArchiveData CurrentArchiveData { get; private set; }
        //存档预览数据
        public PlayerArchivePreviewData ArchivePreviewData { get; private set; }
        public int CurrentSaveIndex { get; private set; } = -1;
        private LocalCacheManager m_localCacheManager;
        private float m_playTime=0;
        private float m_lastSaveInterval=0;
    }
}