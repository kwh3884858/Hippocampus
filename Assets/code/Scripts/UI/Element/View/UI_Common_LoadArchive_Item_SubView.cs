using TMPro;
using UI.UIComponent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Common_LoadArchive_Item_SubView : UIElementBase
    {
		public const string VIEW_NAME = "UI_Common_LoadArchive_Item";

        public void Init (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Common_LoadArchive_Item;
		[HideInInspector] public RectTransform m_go_archiveInfo;
		[HideInInspector] public TextMeshProUGUI m_lbl_playTime_TextMeshProUGUI;

		[HideInInspector] public Image m_img_cg_Image;

		[HideInInspector] public Image m_sb_bar_Image;
		[HideInInspector] public Scrollbar m_sb_bar_Scrollbar;

		[HideInInspector] public Image m_sv_list_Image;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public RectTransform m_UI_Menu_StoryPoint_Item;
		[HideInInspector] public TextMeshProUGUI m_lbl_epName_TextMeshProUGUI;

		[HideInInspector] public TextMeshProUGUI m_lbl_lastPlayTime_TextMeshProUGUI;

		[HideInInspector] public Image m_btn_selectArchive_Image;
		[HideInInspector] public Button m_btn_selectArchive_Button;

		[HideInInspector] public RectTransform m_go_addNewArchive;
		[HideInInspector] public Image m_btn_addNewArchive_Image;
		[HideInInspector] public Button m_btn_addNewArchive_Button;



        private void UIFinder()
        {       
			m_UI_Common_LoadArchive_Item = gameObject.GetComponent<RectTransform>();
			m_go_archiveInfo = FindUI<RectTransform>(gameObject.transform ,"bg/go_archiveInfo");
			m_lbl_playTime_TextMeshProUGUI = FindUI<TextMeshProUGUI>(gameObject.transform ,"bg/go_archiveInfo/playTime/lbl_playTime");

			m_img_cg_Image = FindUI<Image>(gameObject.transform ,"bg/go_archiveInfo/img_cg");

			m_sb_bar_Image = FindUI<Image>(gameObject.transform ,"bg/go_archiveInfo/sb_bar");
			m_sb_bar_Scrollbar = FindUI<Scrollbar>(gameObject.transform ,"bg/go_archiveInfo/sb_bar");

			m_sv_list_Image = FindUI<Image>(gameObject.transform ,"bg/go_archiveInfo/sv_list");
			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"bg/go_archiveInfo/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"bg/go_archiveInfo/sv_list");

			m_UI_Menu_StoryPoint_Item = FindUI<RectTransform>(gameObject.transform ,"bg/go_archiveInfo/sv_list/Viewport/Content/UI_Menu_StoryPoint_Item");
			m_lbl_epName_TextMeshProUGUI = FindUI<TextMeshProUGUI>(gameObject.transform ,"bg/go_archiveInfo/lbl_epName");

			m_lbl_lastPlayTime_TextMeshProUGUI = FindUI<TextMeshProUGUI>(gameObject.transform ,"bg/go_archiveInfo/lbl_lastPlayTime");

			m_btn_selectArchive_Image = FindUI<Image>(gameObject.transform ,"bg/go_archiveInfo/btn_selectArchive");
			m_btn_selectArchive_Button = FindUI<Button>(gameObject.transform ,"bg/go_archiveInfo/btn_selectArchive");

			m_go_addNewArchive = FindUI<RectTransform>(gameObject.transform ,"bg/go_addNewArchive");
			m_btn_addNewArchive_Image = FindUI<Image>(gameObject.transform ,"bg/go_addNewArchive/btn_addNewArchive");
			m_btn_addNewArchive_Button = FindUI<Button>(gameObject.transform ,"bg/go_addNewArchive/btn_addNewArchive");


			BindEvent();
        }

        #endregion
    }
}