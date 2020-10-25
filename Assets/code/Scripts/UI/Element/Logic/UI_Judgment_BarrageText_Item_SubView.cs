using System;
using System.Collections;
using System.Collections.Generic;
using code.StarPlatinum.EventManager;
using StarPlatinum;
using StarPlatinum.EventManager;
using StarPlatinum.Service;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Judgment_BarrageText_Item_SubView : UIElementBase
    {
        private bool m_isSpecial;
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
            if (isSpecial!=m_isSpecial)
            {
                m_isSpecial = isSpecial;
                SetBG();
            }
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

        private void SetBG()
        {
            if (m_isSpecial)
            {
                PrefabManager.Instance.SetImage(m_img_bg_Image, UIRes.SpecialBarrageImg);
            }
            else
            {
                PrefabManager.Instance.SetImage(m_img_bg_Image, UIRes.NormalBarrageImg);
            }
        }
        
        private void OnSlash(object sender, ControversyEvent e)
        {
            Vector3 myPos= CameraService.Instance.GetMainCameraComponent().WorldToScreenPoint(m_root_RectTransform.position);
            Vector3 otherPos = CameraService.Instance.GetMainCameraComponent().WorldToScreenPoint(e.Pos);
            if (Mathf.Abs(otherPos.x - myPos.x) <= m_width)
            {
                EventManager.Instance.SendEvent(new ControversyBarrageSlashEvent(){SubView = this});
            }
        }

    }
}