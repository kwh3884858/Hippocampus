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

}