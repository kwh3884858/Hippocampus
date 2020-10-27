using System;

namespace StarPlatinum
{
    public class PlayerLoadArchiveEvent: EventArgs
    {
        
    }

    //预存档事件，通知其他模块更新存档数据
    public class PlayerPreSaveArchiveEvent : EventArgs
    {
        
    }

    public class PlayerSaveArchiveEvent : EventArgs
    {
        
    }

    public class PlayerDeleteArchiveEvent: EventArgs
    { }
    public class SettingStateEvent : EventArgs
    {
        public bool IsShow;
    }

    public class BookMarkEvent : EventArgs
    {
        /// <summary>是否停止书签功能</summary>
        public bool IsStop;
    }

}