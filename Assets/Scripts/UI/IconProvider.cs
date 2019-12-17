using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extentions;
using UI.Panels;
using UnityEngine;


namespace UI
{
 
    public class IconProvider : MonoBehaviour
    {
        [Serializable]
        private class SpritePair
        {
            public string Name => m_name;
            public Sprite Sprite => m_sprite;

            public SpritePair() { }
            public SpritePair(string name)
            {
                m_name = name;
            }

            [SerializeField]
            private string m_name = string.Empty;
            [SerializeField]
            private Sprite m_sprite = null;
        }

        [SerializeField] private List<SpritePair> m_itemIcon = new List<SpritePair>();
    }
}
