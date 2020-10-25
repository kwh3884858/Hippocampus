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
		[HideInInspector] public RectTransform m_UI_Judgment_ControversyBarrage_Item;
		[HideInInspector] public HorizontalLayoutGroup m_pl_barrage_HorizontalLayoutGroup;
		[HideInInspector] public MakeChildTransparent m_pl_barrage_MakeChildTransparent;

		[HideInInspector] public Image m_img_front_Image;

		[HideInInspector] public HorizontalLayoutGroup m_go_container_HorizontalLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_go_container_ContentSizeFitter;

		[HideInInspector] public Image m_img_behind_Image;



        private void UIFinder()
        {       
			m_UI_Judgment_ControversyBarrage_Item = gameObject.GetComponent<RectTransform>();
			m_pl_barrage_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_barrage");
			m_pl_barrage_MakeChildTransparent = FindUI<MakeChildTransparent>(gameObject.transform ,"pl_barrage");

			m_img_front_Image = FindUI<Image>(gameObject.transform ,"pl_barrage/img_front");

			m_go_container_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_barrage/go_container");
			m_go_container_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_barrage/go_container");

			m_img_behind_Image = FindUI<Image>(gameObject.transform ,"pl_barrage/img_behind");


			BindEvent();
        }

        #endregion
    }
}