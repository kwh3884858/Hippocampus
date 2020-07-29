using System.Collections.Generic;
using Controllers.Subsystems;

namespace LocalCache
{
    public class PlayerArchiveData: LocalCacheBase
    {
        public CGSceneArchiveData CgSceneArchiveData { get; set; }
        public MissionArchiveData MissionArchieData { get; set; }
    }

    public class CGSceneArchiveData : LocalCacheBase
    {
        public Dictionary<int,CGScenePointArchiveInfo> PointInfos { get; set; }
    }

    public class MissionArchiveData : LocalCacheBase
	{
        public string CurrentMission { get; set; }
        public List<string> StoryTriggered { get; set; }
        public Dictionary<string, int> ObjectTriggeredCounter { get; set; }
    }

    public class ReadPlotData : LocalCacheBase
    {
        public List<string> ReadPlotIDs { get; set; }
    }
}