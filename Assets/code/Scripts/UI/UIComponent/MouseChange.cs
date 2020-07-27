using System;
using StarPlatinum;
using StarPlatinum.EventManager;
using UnityEngine;

namespace UI.UIComponent
{
    public class MouseChange : MonoBehaviour
    {
        public Texture2D DefaultCursor = null;
        private void Awake()
        {
            EventManager.Instance.AddEventListener<ChangeCursorEvent> (ChangeCursor);
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveEventListener<ChangeCursorEvent> (ChangeCursor);
        }

        private void ChangeCursor(object sender, ChangeCursorEvent e)
        {
            if (string.IsNullOrEmpty(e.cursorKey))
            {
                Cursor.SetCursor(DefaultCursor, Vector2.zero, CursorMode.Auto);
                return;
            }
            PrefabManager.Instance.LoadAssetAsync<Texture>(e.cursorKey, (result) =>
            {
                Cursor.SetCursor(result.result as Texture2D, Vector2.zero, CursorMode.Auto);
                
            });
        }
    }
}