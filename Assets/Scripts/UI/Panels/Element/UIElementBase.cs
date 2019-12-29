using Const;
using StarPlatinum;
using UnityEngine;

namespace UI.Panels.Element
{
    public class UIElementBase : MonoBehaviour
    {
        public void ClickSound(int num)
        {
            SoundService.Instance.PlayEffect(SoundNameConst.UIClickName+num);
        }
    }
}