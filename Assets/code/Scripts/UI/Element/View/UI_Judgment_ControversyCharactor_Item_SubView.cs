using TMPro;
using UI.UIComponent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Judgment_ControversyCharactor_Item_SubView : UIElementBase
    {
		public const string VIEW_NAME = "UI_Judgment_ControversyCharactor_Item";

        public void Init (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Judgment_ControversyCharactor_Item_Animator;
		[HideInInspector] public UI_Judgment_ControversyCharactor_Item_SubView m_UI_Judgment_ControversyCharactor_Item_UI_Judgment_ControversyCharactor_Item_SubView;



        private void UIFinder()
        {       
			m_UI_Judgment_ControversyCharactor_Item_Animator = gameObject.GetComponent<Animator>();
			m_UI_Judgment_ControversyCharactor_Item_UI_Judgment_ControversyCharactor_Item_SubView = gameObject.GetComponent<UI_Judgment_ControversyCharactor_Item_SubView>();


			BindEvent();
        }

        #endregion
    }
}