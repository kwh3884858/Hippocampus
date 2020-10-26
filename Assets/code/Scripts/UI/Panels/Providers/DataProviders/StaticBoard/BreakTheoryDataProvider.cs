using System;

namespace UI.Panels.Providers.DataProviders.StaticBoard
{
    public class BreakTheoryDataProvider : DataProvider
    {
        public EnumBreakTheoryType Type;
        public string ImgKey;
        public Action CloseCallback;
    }
}