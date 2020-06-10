using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Panels
{
    public enum UIPanelUnLoadMode
    {
        Disabled = 0,
        Destroy = 1
    }

    public enum UIPanelShowMode
    {
        Instantly = 0,
        Deferred = 1
    }

    [System.Serializable]
    public class UIPanelSettings
    {
        public UIPanelType PanelType
        {
            get { return m_panelType; }
            set { m_panelType = value; }
        }

        public UIPanelUnLoadMode MUnLoadMode
        {
            get { return m_unLoadMode; }
            set { m_unLoadMode = value; }
        }

        public bool ShowOnStart
        {
            get { return m_showOnStart; }
            set { m_showOnStart = value; }
        }

        [SerializeField]
        private UIPanelType m_panelType = UIPanelType.None;
        [SerializeField]
        private UIPanelUnLoadMode m_unLoadMode = UIPanelUnLoadMode.Destroy;
        [SerializeField]
        private bool m_showOnStart;

        public UIPanelSettings(UIPanelType _panelType, UIPanelUnLoadMode _unLoadMode = UIPanelUnLoadMode.Destroy, bool _showOnStart = false)
        {
            m_panelType = _panelType;
            m_unLoadMode = _unLoadMode;
            m_showOnStart = _showOnStart;
        }
    }

    public static class UIPanelSettingProvider
    {
        public static List<UIPanelSettings> BattleInfo = new List<UIPanelSettings>()
        {
            new UIPanelSettings(UIPanelType.JoystickPanel,UIPanelUnLoadMode.Destroy),

        };
        public static List<UIPanelSettings> MenuInfo = new List<UIPanelSettings>()
        {
            new UIPanelSettings(UIPanelType.MainManuPanel,UIPanelUnLoadMode.Destroy,true),
                  };
        public static List<UIPanelSettings> StaticBoardInfo = new List<UIPanelSettings>()
        {
            new UIPanelSettings(UIPanelType.TalkPanel,UIPanelUnLoadMode.Disabled),
            new UIPanelSettings(UIPanelType.Tipspanel,UIPanelUnLoadMode.Disabled),
            new UIPanelSettings(UIPanelType.Evidencepanel,UIPanelUnLoadMode.Destroy),
            new UIPanelSettings(UIPanelType.OptionsPanel,UIPanelUnLoadMode.Disabled),
            new UIPanelSettings(UIPanelType.LoadGamePanel,UIPanelUnLoadMode.Destroy),
            new UIPanelSettings(UIPanelType.Singleevidenceselectpanel,UIPanelUnLoadMode.Disabled),
        };
        
    }
}