using System;
using System.Collections.Generic;
using Controllers.Subsystems.Story;

namespace UI.Panels.Providers.DataProviders.StaticBoard
{
    public class RecordDataProvider: DataProvider
    {
        public List<RecordData> Records { get; set; }
    }
}