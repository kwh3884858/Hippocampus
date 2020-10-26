using TMPro;
using UI.UIComponent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Judgment_ControversyBarrage_Item_SubView : UIElementBase
    {
		public const string VIEW_NAME = "UI_Judgment_ControversyBarrage_Item";

        public void Init (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public UI_Judgment_ControversyBarrage_Item_SubView m_UI_Judgment_ControversyBarrage_Item_UI_Judgment_ControversyBarrage_Item_SubView;

		[HideInInspector] public HorizontalLayoutGroup m_pl_barrage_HorizontalLayoutGroup;
		[HideInInspector] public MakeChildTransparent m_pl_barrage_MakeChildTransparent;

		[HideInInspector] public Image m_img_front_Image;

		[HideInInspector] public HorizontalLayoutGroup m_go_container_HorizontalLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_go_container_ContentSizeFitter;
		[HideInInspector] public Image m_go_container_Image;

		[HideInInspector] public RectTransform m_go_behind;


        private void UIFinder()
        {       
			m_UI_Judgment_ControversyBarrage_Item_UI_Judgment_ControversyBarrage_Item_SubView = gameObject.GetComponent<UI_Judgment_ControversyBarrage_Item_SubView>();

			m_pl_barrage_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_barrage");
			m_pl_barrage_MakeChildTransparent = FindUI<MakeChildTransparent>(gameObject.transform ,"pl_barrage");

			m_img_front_Image = FindUI<Image>(gameObject.transform ,"pl_barrage/img_front");

			m_go_container_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_barrage/go_container");
			m_go_container_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_barrage/go_container");
			m_go_container_Image = FindUI<Image>(gameObject.transform ,"pl_barrage/go_container");

			m_go_behind = FindUI<RectTransform>(gameObject.transform ,"pl_barrage/go_behind");

			BindEvent();
        }

        #endregion
    }
}