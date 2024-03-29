﻿using System.Collections.Generic;
using Config.GameRoot;
using Controllers.Subsystems;
using GamePlay.Stage;


namespace LocalCache
{
    public class PlayerArchiveData : LocalCacheBase
    {
        public PlayerBasicInfoData PlayerBasicInfoData { get; set; }
        public CGSceneArchiveData CgSceneArchiveData { get; set; }
        public MissionArchiveData MissionArchieData { get; set; }
        public EvidenceArchiveData EvidenceArchiveData { get; set; }
        public TipsArchiveData TipsArchiveData { get; set; }
        public SoundServiceArchiveData SoundServiceArchiveData { get; set; }
        public StoryArchiveData StoryArchiveData { get; set; }
    }

    public class PlayerBasicInfoData : LocalCacheBase
    {
    }

    public class CGSceneArchiveData : LocalCacheBase
    {
        public bool IsInCgScene { get; set; }
        public string CgSceneId { get; set; }
        public Dictionary<int, CGScenePointArchiveInfo> PointInfos { get; set; }
    }

    public class MissionArchiveData : LocalCacheBase
    {
        public float PlayerPositionX { get; set; } = 0;
        public float PlayerPositionY { get; set; } = 0;
        public float PlayerPositionZ { get; set; } = 0;
        public SceneLookupEnum CurrentGameScene { get; set; } = ConfigRoot.Instance.StartScene;
        public MissionEnum CurrentMission { get; set; } = ConfigRoot.Instance.StartMission;
        public List<string> StoryTriggered { get; set; }
        public Dictionary<string, int> ObjectTriggeredCounter { get; set; }
    }

    public class ReadPlotData : LocalCacheBase
    {
        public List<string> ReadPlotIDs { get; set; }
    }

    public class EvidenceArchiveData : LocalCacheBase
    {
        public Dictionary<string, Evidence.SingleEvidenceData> EvidenceList { get; set; }
        
        public List<string> RemovedEvidenceStory { get; set; }
    }

    public class TipsArchiveData : LocalCacheBase
    {
        public Dictionary<string, Tips.TipData> TipsList { get; set; }
    }

    public class SoundServiceArchiveData : LocalCacheBase
    {
        public string BgmName { get; set; }
        public string effectName { get; set; }
    }

    public class StoryArchiveData : LocalCacheBase
    {
        public Dictionary<string,List<string>> ReadLabels { get; set; }
        
        public List<LogInfo> LogInfos { get; set; }
    }
}