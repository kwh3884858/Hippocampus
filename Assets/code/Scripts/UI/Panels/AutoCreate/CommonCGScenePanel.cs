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
    public partial class CommonCGScenePanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public RectTransform m_go_CGSceneItems;
		[HideInInspector] public Image m_btn_exit_Image;
		[HideInInspector] public Button m_btn_exit_Button;

		[HideInInspector] public Image m_btn_back_Image;
		[HideInInspector] public Button m_btn_back_Button;
		[HideInInspector] public Flashing m_btn_back_Flashing;

		[HideInInspector] public Image m_btn_showImg_Image;
		[HideInInspector] public Button m_btn_showImg_Button;

		[HideInInspector] public Image m_img_showImg_Image;


         private CommonCGSceneModel m_model;

         private void UIFinder()
         {
			m_go_CGSceneItems = FindUI<RectTransform>(transform ,"go_CGSceneItems");
			m_btn_exit_Image = FindUI<Image>(transform ,"btn_exit");
			m_btn_exit_Button = FindUI<Button>(transform ,"btn_exit");

			m_btn_back_Image = FindUI<Image>(transform ,"btn_back");
			m_btn_back_Button = FindUI<Button>(transform ,"btn_back");
			m_btn_back_Flashing = FindUI<Flashing>(transform ,"btn_back");

			m_btn_showImg_Image = FindUI<Image>(transform ,"btn_showImg");
			m_btn_showImg_Button = FindUI<Button>(transform ,"btn_showImg");

			m_img_showImg_Image = FindUI<Image>(transform ,"btn_showImg/img_showImg");


m_model = new CommonCGSceneModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}