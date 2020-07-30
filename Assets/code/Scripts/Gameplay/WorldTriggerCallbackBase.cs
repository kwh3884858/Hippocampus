using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GamePlay.EventTrigger
{
    public abstract class WorldTriggerCallbackBase : MonoBehaviour
    {
        
        // Start is called before the first frame update
        void Start()
        {
			if (m_isInitialized == false) {
                if (m_worldTrigger == null) {
                    m_worldTrigger = GetComponent<WorldTrigger> ();
                    if (m_worldTrigger == null) {
                        gameObject.AddComponent<WorldTrigger> ();
                    }
                }

                m_worldTrigger.Callback = Callback;
                AfterStart ();
                m_isInitialized = true;
            }
        }

        private WorldTrigger m_worldTrigger;
        private bool m_isInitialized = false;
        
        protected abstract void Callback();
        protected virtual void AfterStart() { }
    }
}