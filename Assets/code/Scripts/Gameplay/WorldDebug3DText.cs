using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Utility
{
    public class WorldDebug3DText : MonoBehaviour
    {
        public void SetText(string text)
        {
            if (m_3dTextMesh != null)
            {
                m_3dTextMesh.text = text;
            }
        }

        public TextMesh m_3dTextMesh;
    }
}