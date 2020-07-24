using System.Collections.Generic;
using Controllers.Subsystems;

namespace LocalCache
{
    public class PlayerArchiveData: LocalCacheBase
    {
        public CGSceneArchiveData CgSceneArchiveData { get; set; }
    }

    public class CGSceneArchiveData : LocalCacheBase
    {
        public Dictionary<int,CGScenePointInfo> PointInfos { get; set; }
    }

    public class ReadPlotData : LocalCacheBase
    {
        public List<string> ReadPlotIDs { get; set; }
    }
}