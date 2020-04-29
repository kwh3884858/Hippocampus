using System;
using System.Collections.Generic;
using UI.Panels.StaticBoard;

namespace UI.Panels.Providers.DataProviders.StaticBoard
{
    public class OptionsDataProvider: DataProvider
    {
        public Action<string> Callback { get; set; }
        public List<Option> Options { get; set; }
    }
}