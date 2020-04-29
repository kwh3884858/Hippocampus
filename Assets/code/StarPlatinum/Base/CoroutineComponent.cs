using System;
using System.Collections;


using UnityEngine;

namespace StarPlatinum
{
    class CoroutineComponent : MonoBehaviour
    {
        //public delegate IEnumerator DelayToInvokeDo(Action action, float delaySeconds);

        public void InvokeCoroutine(IEnumerator callback)
        {
            StartCoroutine(callback);
        }

    }
}
