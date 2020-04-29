using System.Collections;
using UnityEngine;

namespace UI.Panels
{
    public enum UIPanelState
    {
        WaitData = 1,
        Show = 2,
        Hide = 3,
        Deactivate = 4,
        Initialize = 5,
    }

    public static class UIPanelStateExt
    {
        public static bool IsShow(this UIPanelState state)
        {
            return (state == UIPanelState.Show ||
                    state == UIPanelState.WaitData);
        }
    }
}