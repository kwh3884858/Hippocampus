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

		[HideInInspector] public TextMeshProUGUI m_lbl_text_TextMeshProUGUI;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;

		[HideInInspector] public Image m_img_bg_Image;



        private void UIFinder()
        {       
			m_UI_Judgment_BarrageText_Item_ContentSizeFitter = gameObject.GetComponent<ContentSizeFitter>();
			m_UI_Judgment_BarrageText_Item_HorizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();

			m_lbl_text_TextMeshProUGUI = FindUI<TextMeshProUGUI>(gameObject.transform ,"lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_text");

			m_img_bg_Image = FindUI<Image>(gameObject.transform ,"lbl_text/img_bg");


			BindEvent();
        }

        #endregion
    }
}