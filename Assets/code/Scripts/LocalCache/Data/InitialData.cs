using System.Collections.Generic;

namespace LocalCache
{
    public class InitialData: LocalCacheBase
    {
    }

    public class PlayerArchivePreviewData : LocalCacheBase
    {
        public List<ArchivePreviewData> ArchivePreviewData { get; set; }
    }

    public class ArchivePreviewData
    { 
        public long SaveTime{ get; set; }
        public string Img{ get; set; }
        public long TotalPlayTime{ get; set; }
        public string EPName { get; set; }
    }
}