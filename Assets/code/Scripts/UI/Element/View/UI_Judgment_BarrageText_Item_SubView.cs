using TMPro;
using UI.UIComponent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Judgment_BarrageText_Item_SubView : UIElementBase
    {
		public const string VIEW_NAME = "UI_Judgment_BarrageText_Item";

        public void Init (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ContentSizeFitter m_UI_Judgment_BarrageText_Item_ContentSizeFitter;
		[HideInInspector] public HorizontalLayoutGroup m_UI_Judgment_BarrageText_Item_HorizontalLayoutGroup;
		[HideInInspector] public UI_Judgment_BarrageText_Item_SubView m_UI_Judgment_BarrageText_Item_UI_Judgment_BarrageText_Item_SubView;
		[HideInInspector] public Image m_UI_Judgment_BarrageText_Item_Image;

		[HideInInspector] public TextMeshProUGUI m_lbl_text_TextMeshProUGUI;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;



        private void UIFinder()
        {       
			m_UI_Judgment_BarrageText_Item_ContentSizeFitter = gameObject.GetComponent<ContentSizeFitter>();
			m_UI_Judgment_BarrageText_Item_HorizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
			m_UI_Judgment_BarrageText_Item_UI_Judgment_BarrageText_Item_SubView = gameObject.GetComponent<UI_Judgment_BarrageText_Item_SubView>();
			m_UI_Judgment_BarrageText_Item_Image = gameObject.GetComponent<Image>();

			m_lbl_text_TextMeshProUGUI = FindUI<TextMeshProUGUI>(gameObject.transform ,"lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_text");


			BindEvent();
        }

        #endregion
    }
}