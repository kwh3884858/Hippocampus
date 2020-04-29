using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarPlatinum
{
    public class TimerServiceMono : MonoBehaviour
    {
        TimerService.TimerCallback m_callback = null;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            m_callback?.Invoke();
        }

        public void SetTimerCallback(TimerService.TimerCallback callback)
        {
            m_callback = callback;
        }
    }
}