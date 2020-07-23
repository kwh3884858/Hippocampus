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

        public virtual void BindEvent()
        {
            
        }

        protected T FindUI<T>(Transform transform,string path)
        { 
            var t = transform.Find(path);
            if (t == null)
            {
                Debug.LogError($"找不到该物体:{path}");
                return default(T);
            }

            return t.GetComponent<T>();
        }
        public RectTransform m_root_RectTransform;
    }
}