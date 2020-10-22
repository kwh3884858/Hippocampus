using System;

namespace StarPlatinum
{
    public class ChangeCursorEvent: EventArgs
    {
        public string cursorKey;
    }

    public class CGScenePointInfoChangeEvent : EventArgs
    {
        
    }

    public class CGSceneCloseEvent : EventArgs
    {
        public string m_cgSceneId = null;
    }
}