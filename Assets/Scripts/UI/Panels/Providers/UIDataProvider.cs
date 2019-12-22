
using Config;
using Controllers;

using StarPlatinum;
using UI.Modules;

namespace UI.Panels.Providers
{

    public class UIDataProvider
    {
        public ControllerManager ControllerManager
        {
            get
            {
                return Data.ControllerManager;
            }
        }

        public ConfigProvider ConfigProvider
        {
            get { return Data.ConfigProvider; }
        }

        public GameRunTimeData Data { get; set; }
        public IconProvider IconProvider
        {
            get; set;
        }

        public SoundService SoundService
        {
            get;set;
        }

        public UIModuleStaticBoard StaticBoard { get; set; }
    }
}
