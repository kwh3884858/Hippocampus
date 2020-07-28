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
    public partial class CommonLogPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_img_bg_Image;

		[HideInInspector] public Image m_sv_list_Image;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public Image m_btn_back_Image;
		[HideInInspector] public Button m_btn_back_Button;


         private CommonLogModel m_model;

         private void UIFinder()
         {
			m_img_bg_Image = FindUI<Image>(transform ,"img_bg");

			m_sv_list_Image = FindUI<Image>(transform ,"sv_list");
			m_sv_list_ScrollRect = FindUI<ScrollRect>(transform ,"sv_list");
			m_sv_list_ListView = FindUI<ListView>(transform ,"sv_list");

			m_btn_back_Image = FindUI<Image>(transform ,"btn_back");
			m_btn_back_Button = FindUI<Button>(transform ,"btn_back");


m_model = new CommonLogModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}