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

        public UIPanelUnLoadMode UnLoadMode
        {
            get { return unLoadMode; }
            set { unLoadMode = value; }
        }

        public bool ShowOnStart
        {
            get { return m_showOnStart; }
            set { m_showOnStart = value; }
        }

        [SerializeField]
        private UIPanelType m_panelType = UIPanelType.None;
        [SerializeField]
        private UIPanelUnLoadMode unLoadMode = UIPanelUnLoadMode.Destroy;
        [SerializeField]
        private bool m_showOnStart;
    }
}