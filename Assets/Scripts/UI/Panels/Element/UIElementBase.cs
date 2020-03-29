using Const;
using StarPlatinum;
using UnityEngine;

namespace UI.Panels.Element
{
    public abstract class UIElementBase : MonoBehaviour
    {
        
        public void ClickSound(int num)
        {
            SoundService.Instance.PlayEffect(SoundNameConst.UIClickName+num);
        }
    }
}