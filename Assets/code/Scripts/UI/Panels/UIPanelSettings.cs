using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Panels
{
    public enum UIPanelLayer
    {
        Layer1=1,
        Layer2=2,
        Layer3=3,
        Layer4=4,
        Layer5=5,
        Layer6=6,
        Layer7=7,
        Layer8=8,
        Layer9=9,

    }
    public enum UIPanelUnLoadMode
    {
        //在场景中隐藏
        Disabled = 0,
        //直接销毁
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

        public UIPanelLayer Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }

        private UIPanelType m_panelType = UIPanelType.None;
        //关闭方式
        private UIPanelUnLoadMode m_unLoadMode = UIPanelUnLoadMode.Destroy;
        //是否在Module加载后立刻显示
        private bool m_showOnStart;

        private UIPanelLayer m_layer;

        public UIPanelSettings(UIPanelType _panelType, UIPanelUnLoadMode _unLoadMode = UIPanelUnLoadMode.Destroy, bool _showOnStart = false,UIPanelLayer _layer = UIPanelLayer.Layer5)
        {
            m_panelType = _panelType;
            m_unLoadMode = _unLoadMode;
            m_showOnStart = _showOnStart;
            m_layer = _layer;
        }
    }

    public static class UIPanelSettingProvider
    {
        public static List<UIPanelSettings> BattleInfo = new List<UIPanelSettings>()
        {
            new UIPanelSettings(UIPanelType.JoystickPanel,UIPanelUnLoadMode.Destroy,false,UIPanelLayer.Layer2),
            new UIPanelSettings(UIPanelType.UIGameplayPromptwidgetPanel,UIPanelUnLoadMode.Disabled, false, UIPanelLayer.Layer3),
            new UIPanelSettings(UIPanelType.UICommonGameplayTransitionPanel,UIPanelUnLoadMode.Disabled,false,UIPanelLayer.Layer5),
            new UIPanelSettings(UIPanelType.UIJudgmentControversyPanel,UIPanelUnLoadMode.Destroy,false, UIPanelLayer.Layer4),
        };
        //Switch MainMenuPanel
        public static List<UIPanelSettings> MenuInfo = new List<UIPanelSettings>()
        {
            //new UIPanelSettings(UIPanelType.MainManuPanel,UIPanelUnLoadMode.Destroy,true),
            new UIPanelSettings(UIPanelType.MGStartmanuMainmanuPanel,UIPanelUnLoadMode.Destroy,true),
                  };
        public static List<UIPanelSettings> StaticBoardInfo = new List<UIPanelSettings>()
        {
            new UIPanelSettings(UIPanelType.UICommonGameplayPanel,UIPanelUnLoadMode.Disabled, false, UIPanelLayer.Layer3),
            new UIPanelSettings(UIPanelType.TalkPanel,UIPanelUnLoadMode.Disabled,false,UIPanelLayer.Layer4),
            new UIPanelSettings(UIPanelType.Tipspanel,UIPanelUnLoadMode.Disabled),
            new UIPanelSettings(UIPanelType.Evidencepanel,UIPanelUnLoadMode.Destroy),
            new UIPanelSettings(UIPanelType.UIMapcanvasPanel,UIPanelUnLoadMode.Destroy),
            new UIPanelSettings(UIPanelType.UICommonMapsTipsEvidencesPanel,UIPanelUnLoadMode.Destroy),
            new UIPanelSettings(UIPanelType.OptionsPanel,UIPanelUnLoadMode.Disabled),
            new UIPanelSettings(UIPanelType.LoadGamePanel,UIPanelUnLoadMode.Destroy),
            new UIPanelSettings(UIPanelType.Singleevidenceselectpanel,UIPanelUnLoadMode.Disabled),
            new UIPanelSettings(UIPanelType.Tipgetpanel,UIPanelUnLoadMode.Destroy),
            new UIPanelSettings(UIPanelType.UICommonESCMainMenuPanel,UIPanelUnLoadMode.Disabled),
            new UIPanelSettings(UIPanelType.UICommonLoadarchivePanel),
            new UIPanelSettings(UIPanelType.UICommonLogPanel,UIPanelUnLoadMode.Destroy),
            new UIPanelSettings(UIPanelType.UICommonCgscenePanel,UIPanelUnLoadMode.Destroy,false,UIPanelLayer.Layer3),
            new UIPanelSettings(UIPanelType.UICommonAssistantPanel,UIPanelUnLoadMode.Destroy,false,UIPanelLayer.Layer5),
            new UIPanelSettings(UIPanelType.UICommonDetectiveNotesPanel,UIPanelUnLoadMode.Destroy,false,UIPanelLayer.Layer8),
            new UIPanelSettings(UIPanelType.UICommonBookmarkPanel,UIPanelUnLoadMode.Destroy,false,UIPanelLayer.Layer8),
            new UIPanelSettings(UIPanelType.UICommonBreaktheoryPanel,UIPanelUnLoadMode.Destroy,false,UIPanelLayer.Layer8),
            new UIPanelSettings(UIPanelType.UICommonSettingPanel,UIPanelUnLoadMode.Disabled,false,UIPanelLayer.Layer5),

        };

    }
}
