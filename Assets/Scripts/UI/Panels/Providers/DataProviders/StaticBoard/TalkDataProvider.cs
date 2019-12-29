using System;

namespace UI.Panels.Providers.DataProviders.StaticBoard
{
    public class TalkDataProvider: DataProvider
    {
        public string ID { get; set; }
        
        public Action OnTalkEnd { get; set; }
    }
}