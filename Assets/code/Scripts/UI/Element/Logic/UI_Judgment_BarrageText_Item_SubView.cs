using System;
using System.Collections;
using System.Collections.Generic;
using code.StarPlatinum.EventManager;
using StarPlatinum;
using StarPlatinum.EventManager;
using StarPlatinum.Services;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Judgment_BarrageText_Item_SubView : UIElementBase
    {
        public BarrageTextItem BarrageInfo { get; private set; }
        private float m_width;
        public override void BindEvent()
        {
            base.BindEvent();
            EventManager.Instance.AddEventListener<ControversyEvent>(OnSlash);
        }

        public void OnDestroy()
        {
            EventManager.Instance.RemoveEventListener<ControversyEvent>(OnSlash);

        }

        public void SetInfo(BarrageTextItem item,bool isSpecial)
        {
            BarrageInfo = item;
            
            m_lbl_text_TextMeshProUGUI.text = item.Text;
            m_lbl_text_TextMeshProUGUI.color = GetColor(isSpecial, item.IsHighLight);
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_UI_Judgment_BarrageText_Item_HorizontalLayoutGroup.rectTransform());
            m_width = m_lbl_text_TextMeshProUGUI.GetPreferredValues().x;
        }

        private Color GetColor(bool isSpecial, bool isHighlight)
        {
            if (isSpecial)
            {
                if (isHighlight)
                {
                    return GameRunTimeData.Instance.ColorProvider.SpecialBarrageHighlightColor;
                }
                else
                {
                    return GameRunTimeData.Instance.ColorProvider.SpecialBarrageColor;
                }
            }
            else
            {
                if (isHighlight)
                {
                    return GameRunTimeData.Instance.ColorProvider.NormalBarrageHighlightColor;
                }
                else
                {
                    return GameRunTimeData.Instance.ColorProvider.NormalBarrageColor;
                }
            }
        }

        private void OnSlash(object sender, ControversyEvent e)
        {
            if (e.Pos.x>=m_root_RectTransform.position.x && e.Pos.x - m_root_RectTransform.position.x <= m_width)
            {
                //TODO:斩击动画
                EventManager.Instance.SendEvent(new ControversyBarrageSlashEvent(){SubView = this});
            }
        }

    }
}