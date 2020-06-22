using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GamePlay.EventTrigger
{
    public abstract class WorldTriggerCallbackBase : MonoBehaviour
    {
        private WorldTrigger m_worldTrigger;

        // Start is called before the first frame update
        void Start()
        {
            if (m_worldTrigger == null)
            {
                m_worldTrigger = GetComponent<WorldTrigger>();
                if (m_worldTrigger == null)
                {
                    gameObject.AddComponent<WorldTrigger>();
                }
            }

            m_worldTrigger.Callback = Callback;
        }

        protected abstract void Callback();
    }
}