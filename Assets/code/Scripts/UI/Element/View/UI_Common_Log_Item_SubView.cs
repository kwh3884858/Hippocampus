using TMPro;
using UI.UIComponent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Common_Log_Item_SubView : UIElementBase
    {
		public const string VIEW_NAME = "UI_Common_Log_Item";

        public void Init (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public UI_Common_Log_Item_SubView m_UI_Common_Log_Item_UI_Common_Log_Item_SubView;

		[HideInInspector] public ContentSizeFitter m_pl_talk_ContentSizeFitter;
		[HideInInspector] public VerticalLayoutGroup m_pl_talk_VerticalLayoutGroup;

		[HideInInspector] public TextMeshProUGUI m_lbl_name_TextMeshProUGUI;

		[HideInInspector] public Image m_img_name_Image;

		[HideInInspector] public TextMeshProUGUI m_lbl_content_TextMeshProUGUI;
		[HideInInspector] public ContentSizeFitter m_lbl_content_ContentSizeFitter;

		[HideInInspector] public VerticalLayoutGroup m_pl_oneLine_VerticalLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_pl_oneLine_ContentSizeFitter;

		[HideInInspector] public TextMeshProUGUI m_lbl_oneLine_TextMeshProUGUI;
		[HideInInspector] public ContentSizeFitter m_lbl_oneLine_ContentSizeFitter;



        private void UIFinder()
        {       
			m_UI_Common_Log_Item_UI_Common_Log_Item_SubView = gameObject.GetComponent<UI_Common_Log_Item_SubView>();

			m_pl_talk_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_talk");
			m_pl_talk_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_talk");

			m_lbl_name_TextMeshProUGUI = FindUI<TextMeshProUGUI>(gameObject.transform ,"pl_talk/name/lbl_name");

			m_img_name_Image = FindUI<Image>(gameObject.transform ,"pl_talk/name/img_name");

			m_lbl_content_TextMeshProUGUI = FindUI<TextMeshProUGUI>(gameObject.transform ,"pl_talk/lbl_content");
			m_lbl_content_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_talk/lbl_content");

			m_pl_oneLine_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_oneLine");
			m_pl_oneLine_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_oneLine");

			m_lbl_oneLine_TextMeshProUGUI = FindUI<TextMeshProUGUI>(gameObject.transform ,"pl_oneLine/lbl_oneLine");
			m_lbl_oneLine_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_oneLine/lbl_oneLine");


			BindEvent();
        }

        #endregion
    }
}