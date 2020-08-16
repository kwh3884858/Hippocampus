using TMPro;
using UI.UIComponent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Common_CGscene_Item_SubView : UIElementBase
    {
		public const string VIEW_NAME = "UI_Common_CGscene_Item";

        public void Init (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public UI_Common_CGscene_Item_SubView m_UI_Common_CGscene_Item_UI_Common_CGscene_Item_SubView;

		[HideInInspector] public Image m_img_cg_Image;

		[HideInInspector] public RectTransform m_go_interactionPoints;


        private void UIFinder()
        {       
			m_UI_Common_CGscene_Item_UI_Common_CGscene_Item_SubView = gameObject.GetComponent<UI_Common_CGscene_Item_SubView>();

			m_img_cg_Image = FindUI<Image>(gameObject.transform ,"img_cg");

			m_go_interactionPoints = FindUI<RectTransform>(gameObject.transform ,"go_interactionPoints");

			BindEvent();
        }

        #endregion
    }
}