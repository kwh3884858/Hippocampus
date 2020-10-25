using System;
using System.Net.Mime;
using Boo.Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIComponent
{
    public class MakeChildTransparent: MonoBehaviour
    {
        [SerializeField]
        private int m_alpha = 0;
        private bool m_isInited = false;
        private bool m_isTransparent = false;
        private float m_defualtAlpha;
        private List<Image> m_imgs =new List<Image>();
        private List<float> m_defualtImgColor = new List<float>();
        private List<TextMeshProUGUI> m_txts = new List<TextMeshProUGUI>();
        private List<float> m_defualtTxtColor = new List<float>();

        private void Awake()
        {
            m_imgs.AddRange(GetComponentsInChildren<Image>());
            foreach (var image in m_imgs)
            {
                m_defualtImgColor.Add(image.color.a);
            }
            m_txts.AddRange(GetComponentsInChildren<TextMeshProUGUI>());
            foreach (var txt in m_txts)
            {
                m_defualtTxtColor.Add(txt.color.a);
            }
            m_isInited = true;
            m_defualtAlpha = (float)m_alpha / 255;
            if (m_isTransparent == true)
            {
                Transparent(m_isTransparent,true);
            }
        }

        public void Transparent(bool isTransparent,bool force = false)
        {
            if (force && m_isTransparent == isTransparent)
            {
                return;
            }

            if (!m_isInited)
            {
                return;
            }

            Color color;
            for(int i=0;i<m_imgs.Count;i++)
            {
                color = m_imgs[i].color;
                if (isTransparent)
                {
                    color.a = m_defualtAlpha;
                }
                else
                {
                    color.a = m_defualtImgColor[i];
                }
                m_imgs[i].color = color;
            }
            
            for(int i=0;i<m_txts.Count;i++)
            {
                color = m_txts[i].color;
                if (isTransparent)
                {
                    color.a = m_defualtAlpha;
                }
                else
                {
                    color.a = m_defualtTxtColor[i];
                }
                m_txts[i].color = color;
            }
        }
    }
}