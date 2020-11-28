using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Const;
using Controllers.Subsystems.Story;
using StarPlatinum;
using TMPro;
using UI.Panels.Element;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;
using UnityEngine.UI;
using UI.UIComponent;

namespace UI.Panels
{
    public partial class CommonOptionsPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public Image m_sv_list_Image;

		[HideInInspector] public VerticalLayoutGroup m_pl_content_VerticalLayoutGroup;

		[HideInInspector] public Image m_btn_history_Image;
		[HideInInspector] public Button m_btn_history_Button;

		[HideInInspector] public RectTransform m_pl_subPanel;

         private CommonOptionsModel m_model;

         private void UIFinder()
         {
			m_sv_list_ScrollRect = FindUI<ScrollRect>(transform ,"sv_list");
			m_sv_list_Image = FindUI<Image>(transform ,"sv_list");

			m_pl_content_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(transform ,"sv_list/Viewport/pl_content");

			m_btn_history_Image = FindUI<Image>(transform ,"btn_history");
			m_btn_history_Button = FindUI<Button>(transform ,"btn_history");

			m_pl_subPanel = FindUI<RectTransform>(transform ,"pl_subPanel");

m_model = new CommonOptionsModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}