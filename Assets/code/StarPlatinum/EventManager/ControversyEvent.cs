using System;
using UI.Panels;
using UI.Panels.Element;
using UnityEngine;

namespace code.StarPlatinum.EventManager
{
    public class ControversyEvent: EventArgs
    {
        public Vector3 Pos;
    }

    public class ControversyBarrageSlashEvent : EventArgs
    {
        public UI_Judgment_BarrageText_Item_SubView SubView;
    }
}