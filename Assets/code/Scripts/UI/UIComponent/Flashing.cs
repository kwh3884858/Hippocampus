using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIComponent
{
    public class Flashing: MonoBehaviour
    {
        public float Interval = 1;

        private float m_curTime = 0;
        private Image m_img;
        private float m_defaultValue;
        private Color m_defaultColor;
        private bool m_isAddTime = true;
        private void Awake()
        {
            m_img = GetComponent<Image>();
            m_defaultValue = m_img.color.a;
            m_defaultColor = m_img.color;
        }

        private void OnEnable()
        {
            m_curTime = 0;
        }

        private void OnDisable()
        {
            m_defaultColor.a = 0;
            m_img.color = m_defaultColor;
        }

        private void Update()
        {
            if (m_isAddTime && m_curTime > Interval)
            {
                m_isAddTime = false;
            }

            if (!m_isAddTime && m_curTime < 0)
            {
                m_isAddTime = true;
            }

            m_curTime += (m_isAddTime ? Time.deltaTime : -Time.deltaTime);

            m_defaultColor.a =(m_curTime / Interval) * m_defaultValue;
            m_img.color = m_defaultColor;
        }
    }
}